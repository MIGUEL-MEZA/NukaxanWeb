Imports System
Imports System.Data
Imports System.Text
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Nulink_Crianza
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveParvada As Integer, CveCaseta As Integer, Semana As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveParvada int=" + CveParvada.ToString)
        sb.Append(" DECLARE @CveCaseta int=" + CveCaseta.ToString)
        sb.Append(" DECLARE @Semana int=" + Semana.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nulink_CrianzaDiaria @CodCliente,@CveParvada,@CveCaseta,@Semana,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveParvada As Integer, CveCaseta As Integer, Semana As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveParvada, CveCaseta, Semana))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    'Public Function FindlstAll(CodCliente As String, CveParvada As Integer, CveCaseta As Integer, Semana As Integer) As List(Of Nulink_Captura_CrianzaModel)
    '    Dim dt As DataTable
    '    Dim lst As New List(Of Nulink_Captura_CrianzaModel)
    '    Try
    '        dt = FindAll(CodCliente, CveParvada, CveCaseta, Semana)
    '        For Each dr As DataRow In dt.Rows
    '            Dim ObjM As Nulink_Captura_CrianzaModel = FillModel(dr)
    '            lst.Add(ObjM)
    '        Next

    '    Catch ex As Exception
    '        strError = CleanSpecialCharacter(ex.Message)
    '    Finally
    '        dt = Nothing
    '    End Try

    '    Return lst
    'End Function
    'Public Function FindById(CodCliente As String, CveParvada As Integer, CveCaseta As Integer) As Nulink_Captura_CrianzaModel
    '    Dim ObjM As New Nulink_Captura_CrianzaModel
    '    Dim dt As DataTable
    '    Try
    '        dt = FindAll(CodCliente, CveParvada, CveCaseta)
    '        If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

    '    Catch ex As Exception
    '        strError = CleanSpecialCharacter(ex.Message)
    '    Finally
    '        dt = Nothing
    '    End Try
    '    Return ObjM
    'End Function
    'Public Function FillModel(dr As DataRow) As Nulink_Captura_CrianzaModel
    '    Dim ObjModel As New Nulink_Captura_CrianzaModel
    '    ObjModel.CodCliente = dr("CodCliente")
    '    ObjModel.CveParvada = dr("CveParvada")
    '    ObjModel.CveCaseta = dr("CveCaseta")
    '    ObjModel.FecCaptura = CDate(dr("FecCaptura")).ToString("dd/MM/yyyy")
    '    ObjModel.CveCiclo = dr("CveCiclo")
    '    ObjModel.Semana = dr("Semana")
    '    ObjModel.Dia = dr("Dia")
    '    ObjModel.PoblacionInicial = dr("PoblacionInicial")
    '    ObjModel.PoblacionFinal = dr("PoblacionFinal")
    '    ObjModel.AvesMuertas = dr("AvesMuertas")
    '    ObjModel.PesoAve = dr("PesoAve")
    '    ObjModel.Uniformidad = dr("Uniformidad")
    '    ObjModel.AlimentoServido = dr("AlimentoServido")
    '    ObjModel.Cal_AlimentoServidoA = dr("Cal_AlimentoServidoA")
    '    ObjModel.Cal_Mortalidad = dr("Cal_Mortalidad")
    '    ObjModel.Cal_MortalidadA = dr("Cal_MortalidadA")
    '    ObjModel.Cal_Supervivencia = dr("Cal_Supervivencia")
    '    ObjModel.Cal_ConsumoAS = dr("Cal_ConsumoAS")
    '    ObjModel.Cal_ConsumoAcumulado = dr("Cal_ConsumoAcumulado")
    '    ObjModel.Cal_ConsumoAD = dr("Cal_ConsumoAD")
    '    ObjModel.Cal_GSP = dr("Cal_GSP")
    '    ObjModel.Cal_ConversionA = dr("Cal_ConversionA")
    '    ObjModel.CodParvada = dr("CodParvada")
    '    ObjModel.CodCaseta = dr("CodCaseta")
    '    ObjModel.NomCiclo = dr("NomCiclo")

    '    'Bitacora
    '    ObjModel.CveEstatus = dr("CveEstatus")
    '    ObjModel.NomEstatus = dr("NomEstatus")
    '    ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
    '    ObjModel.UsuAct = dr("UsuAct")
    '    ObjModel.NomUsuAct = dr("NomUsuAct")

    '    Return ObjModel
    'End Function

    'Public Function SaveModel(CveEquipo As Integer, Valores As String, UsuAct As Int64) As Boolean
    '    Dim sb As New StringBuilder
    '    Dim dt As DataTable
    '    Dim IsResult As Boolean = False
    '    Folio = "0"
    '    Try
    '        sb.Append(" DECLARE @Opcion int=0")
    '        sb.Append(" DECLARE @CveEquipo int=" + CveEquipo.ToString)
    '        sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
    '        sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
    '        sb.Append(" DECLARE @Estatus int=0")
    '        sb.Append(" DECLARE @Mensaje varchar(250)=''")
    '        sb.Append(" DECLARE @Id int=0")

    '        sb.Append(" EXEC spiu_CatEquipos @Opcion,@CveEquipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

    '        dt = execQuery(sb.ToString)
    '        If Not dt Is Nothing And dt.Rows.Count > 0 Then
    '            If dt(0)("Estatus").ToString = "0" Then
    '                Folio = dt(0)("Id").ToString
    '                IsResult = True
    '            Else
    '                Throw New Exception(dt(0)("Mensaje").ToString)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        strError = CleanSpecialCharacter(ex.Message)
    '    Finally
    '        dt = Nothing
    '    End Try
    '    Return IsResult
    'End Function
    'Public Function DeleteModel(CveEquipo As Integer) As Boolean
    '    Dim sb As New StringBuilder
    '    Dim dt As DataTable
    '    Dim IsResult As Boolean = False
    '    Folio = "0"
    '    Try
    '        sb.Append(" DECLARE @Opcion int=2")
    '        sb.Append(" DECLARE @CveEquipo int=" + CveEquipo.ToString)
    '        sb.Append(" DECLARE @Valores varchar(MAX)=''")
    '        sb.Append(" DECLARE @UsuAct bigint=0")
    '        sb.Append(" DECLARE @Estatus int=0")
    '        sb.Append(" DECLARE @Mensaje varchar(250)=''")
    '        sb.Append(" DECLARE @Id int=0")

    '        sb.Append(" EXEC spiu_CatEquipos @Opcion,@CveEquipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

    '        dt = execQuery(sb.ToString)
    '        If Not dt Is Nothing And dt.Rows.Count > 0 Then
    '            If dt(0)("Estatus").ToString = "0" Then
    '                Folio = dt(0)("Id").ToString
    '                IsResult = True
    '            ElseIf dt(0)("Estatus").ToString = "-2" Then
    '                Throw New Exception(New Mensajes().FindById("0", 0, 19).NomMensaje)
    '            Else
    '                Throw New Exception(dt(0)("Mensaje").ToString)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        strError = CleanSpecialCharacter(ex.Message)
    '    Finally
    '        dt = Nothing
    '    End Try
    '    Return IsResult
    'End Function

    Public Function GetSQLSemana(CodCliente As String, CveParvada As Integer, CveCaseta As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveParvada int=" + CveParvada.ToString)
        sb.Append(" DECLARE @CveCaseta int=" + CveCaseta.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nulink_CrianzaSemanal @CodCliente,@CveParvada,@CveCaseta,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAllSemana(CodCliente As String, CveParvada As Integer, CveCaseta As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQLSemana(CodCliente, CveParvada, CveCaseta))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
End Class
