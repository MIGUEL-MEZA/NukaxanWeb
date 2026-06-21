Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class CatCurvasCalibracion
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveCategoriaP As Integer, CveParametro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveCategoriaP  int=" + CveCategoriaP.ToString)
        sb.Append(" DECLARE @CveParametro  int=" + CveParametro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nireo_CurvasCalibracion @CveCategoriaP,@CveParametro,@Estatus Output,@Mensaje Output")


        Return sb.ToString
    End Function
    Public Function FindAll(CveCategoriaP As Integer, CveParametro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveCategoriaP, CveParametro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveCategoriaP As Integer, CveParametro As Integer) As List(Of CatCurvasCalibracionModel)
        Dim dt As DataTable
        Dim lst As New List(Of CatCurvasCalibracionModel)
        Try
            dt = FindAll(CveCategoriaP, CveParametro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As CatCurvasCalibracionModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveCategoriaP As Integer, CveParametro As Integer) As CatCurvasCalibracionModel
        Dim ObjM As New CatCurvasCalibracionModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveCategoriaP, CveParametro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As CatCurvasCalibracionModel
        Dim ObjModel As New CatCurvasCalibracionModel
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.Critico = dr("Critico")
        ObjModel.MinValor = dr("MinValor")
        ObjModel.MaxValor = dr("MaxValor")
        ObjModel.Archivo = dr("Archivo")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.NomParametro = dr("NomParametro")
        ObjModel.NomCritico = dr("NomCritico")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    'Public Function SaveModel(CveParametro As Integer, Valores As String, UsuAct As Int64) As Boolean
    '    Dim sb As New StringBuilder
    '    Dim dt As DataTable
    '    Dim IsResult As Boolean = False
    '    Folio = "0"
    '    Try
    '        sb.Append(" DECLARE @Opcion int=0")
    '        sb.Append(" DECLARE @CveParametro int=" + CveParametro.ToString)
    '        sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
    '        sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
    '        sb.Append(" DECLARE @Estatus int=0")
    '        sb.Append(" DECLARE @Mensaje varchar(250)=''")
    '        sb.Append(" DECLARE @Id int=0")

    '        sb.Append(" EXEC spiu_CatParametros @Opcion,@CveParametro,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    'Public Function DeleteModel(CveParametro As Integer) As Boolean
    '    Dim sb As New StringBuilder
    '    Dim dt As DataTable
    '    Dim IsResult As Boolean = False
    '    Folio = "0"
    '    Try
    '        sb.Append(" DECLARE @Opcion int=2")
    '        sb.Append(" DECLARE @CveParametro int=" + CveParametro.ToString)
    '        sb.Append(" DECLARE @Valores varchar(MAX)=''")
    '        sb.Append(" DECLARE @UsuAct bigint=0")
    '        sb.Append(" DECLARE @Estatus int=0")
    '        sb.Append(" DECLARE @Mensaje varchar(250)=''")
    '        sb.Append(" DECLARE @Id int=0")

    '        sb.Append(" EXEC spiu_CatParametros @Opcion,@CveParametro,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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

End Class
