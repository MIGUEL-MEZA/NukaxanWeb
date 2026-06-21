Imports System.ComponentModel.DataAnnotations
Imports System.Text.Json
Imports System.Text.Json.Serialization
' TODO: Review Newtonsoft.Json nuget package (this contains vulnerabilities. Consider using System.Text.Json instead)
Imports JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute

''' <summary>
''' SpectreData class represents the spectre data of the request to the PredictorAPI (ONLY FOR JSON SERIALIZATION).
''' </summary>
''' <remarks>
''' This class is used to serialize the spectre data to JSON format.
''' </remarks>
Public Class SpectreData
    ''' <summary>
    ''' The spectre data in the format of a list of lists of doubles.
    ''' </summary>
    ''' <returns>The spectre data in the format of a list of lists of doubles.</returns>
    ''' <remarks>
    ''' <example>
    ''' The spectre like this:
    ''' <code>
    ''' "Spectre": [ [1.0759897, ...], ... ]
    ''' </code>
    ''' </example>
    ''' </remarks>
    <JsonPropertyName("Spectre"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property Spectre() As List(Of List(Of Double))
    ''' <summary>
    ''' The model names associated with the spectre data.
    ''' </summary>
    ''' <returns>The model names associated with the spectre data.</returns>
    ''' <remarks>
    ''' <example>
    ''' The model names like this:
    ''' <code>
    ''' "ModelNames": ["Model1", ...]
    ''' </code>
    ''' </example>
    ''' </remarks>
    <JsonPropertyName("ModelNames"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property ModelNames As List(Of String)
#Region "Not used in the current implementation (Only for NukaxanMobile)"
    ''' <summary> NOT USED </summary>
    <JsonPropertyName("CveSesion"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property CveSesion As Object
    ''' <summary> NOT USED </summary>
    <JsonPropertyName("CveMuestra"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property CveMuestra As Object
    ''' <summary> NOT USED </summary>
    <JsonPropertyName("CveCategoriaP"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property CveCategoriaP As Object
    ''' <summary> NOT USED </summary>
    <JsonPropertyName("CveProducto"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property CveProducto As Object
    ''' <summary> NOT USED </summary>
    <JsonPropertyName("CveParametro"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property CveParametro As Object
#End Region
End Class

''' <summary>
''' Represents a table SPECTRE from ForecastEngine database.
''' </summary>
Public Class SPECTREModel
    ''' <summary>
    ''' Unique identifier (primary key).
    ''' </summary>
    ''' <returns>The ID of the record.</returns>
    <JsonPropertyName("id"), JsonIgnore(Condition:=JsonIgnoreCondition.Always)>
    Public Property Id As Long
    ''' <summary>
    ''' Transaction identifier (foreign key).
    ''' </summary>
    ''' <returns>The ID of the associated transaction.</returns>
    <JsonPropertyName("id_trx"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property IdTrx As Long
    ''' <summary>
    ''' Priority identifier (foreign key).
    ''' </summary>
    ''' <returns>The ID of the associated priority.</returns>
    <JsonPropertyName("id_priority"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property IdPriority As Integer
    ''' <summary>
    ''' Status identifier (foreign key).
    ''' </summary>
    ''' <returns>The ID of the associated status.</returns>
    <JsonPropertyName("id_status"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property IdStatus As Integer
    ''' <summary>
    ''' JSON content for the SPECTRE data.
    ''' </summary>
    ''' <returns>A JSON string representing the SPECTRE data.</returns>
    ''' <remarks>For the predictor this is in RAW format; like (with 'S' of 'Spectre' in capital):
    ''' <example>
    ''' The JSON content for the SPECTRE data is like this:
    ''' <code>
    ''' { "Spectre" : [ [ 1.0759897, ...], ... ] }
    ''' </code>
    ''' </example>
    ''' </remarks>
    ''' <see cref="SpectreData"/>
    <JsonPropertyName("spectre_json"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property SpectreJson As SpectreData
    ''' <summary>
    ''' JSON content for the plans.
    ''' </summary>
    ''' <returns>A JSON string representing the plans.</returns>
    ''' <remarks>Predictor normally returns in the format (with 'p' of 'planes' in lowercase):
    ''' <example>
    ''' The spectre like this:
    ''' <code>
    ''' "planes_json": "[3.4600012, ...]"
    ''' </code>
    ''' </example>
    ''' </remarks>
    <JsonPropertyName("planes_json"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property PlanesJson As String
    ''' <summary>
    ''' The file name associated with the model (max 50 characters).
    ''' </summary>
    Private _fileName As String
    ''' <inheritdoc cref="_fileName">''' </inheritdoc>
    ''' <returns>
    ''' The name of the file associated with the model.
    ''' Validated to be less than 50 characters.
    ''' </returns>
    <JsonPropertyName("file_name"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    <MaxLength(100)>
    Public Property FileName As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            If value IsNot Nothing AndAlso value.Length > 100 Then
                Throw New ArgumentException("FileName cannot exceed 100 characters.")
            End If
            _fileName = value
        End Set
    End Property
    ''' <summary>
    ''' The model file stored as a byte array (nullable).
    ''' </summary>
    ''' <returns>The binary data of the UNSB model file.</returns>
    <JsonPropertyName("model_file"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property ModelFile As Byte()
    ''' <summary>
    ''' Model type identifier (foreign key, nullable).
    ''' </summary>
    ''' <returns>The ID of the model type, or null if not assigned.</returns>
    <JsonPropertyName("id_model_type"), JsonIgnore(Condition:=JsonIgnoreCondition.Never)>
    Public Property IdModelType As Integer?
    ''' <summary>
    ''' Date and time when the record was created.
    ''' </summary>
    ''' <returns>The timestamp of when the record was created.</returns>
    <JsonPropertyName("created_at"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property CreatedAt As DateTime
    ''' <summary>
    ''' Date and time when the record was last updated.
    ''' </summary>
    ''' <returns>The timestamp of when the record was last updated.</returns>
    <JsonPropertyName("updated_at"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property UpdatedAt As DateTime
    ''' <summary>
    ''' Date and time when the record was deleted (nullable).
    ''' </summary>
    ''' <returns>The timestamp of when the record was deleted, or null if not deleted.</returns>
    <JsonPropertyName("deleted_at"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property DeletedAt As DateTime?
    ''' <summary>
    ''' Navigation MODEL property for the associated model type (nullable).
    ''' </summary>
    ''' <returns>The model type associated with the record.</returns>
    <JsonPropertyName("id_model_typeNavigation"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property IdModelTypeNavigation As Object = Nothing
    ''' <summary>
    ''' Navigation PRIORITY property for the associated priority (nullable).
    ''' </summary>
    ''' <returns>The priority associated with the record.</returns>
    <JsonPropertyName("id_priorityNavigation"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property IdPriorityNavigation As Object = Nothing
    ''' <summary>
    ''' Navigation STATUS property for the associated status (nullable).
    ''' </summary>
    ''' <returns>The status associated with the record.</returns>
    <JsonPropertyName("id_statusNavigation"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property IdStatusNavigation As Object = Nothing
    ''' <summary>
    ''' Navigation TRANSACTION property for the associated transaction (nullable).
    ''' </summary>
    ''' <returns>The transaction associated with the record.</returns>
    <JsonPropertyName("id_trxNavigation"), JsonIgnore(Condition:=JsonIgnoreCondition.WhenWritingDefault)>
    Public Property IdTrxNavigation As Object = Nothing
End Class
