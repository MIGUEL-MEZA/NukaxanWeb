Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class CatEquipos
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveEquipo As Integer, CveCategoria As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveEquipo  int=" + CveEquipo.ToString)
        sb.Append(" DECLARE @CveCategoria  int=" + CveCategoria.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_CatEquipos @CveEquipo,@CveCategoria,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveEquipo As Integer, CveCategoria As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveEquipo, CveCategoria))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveEquipo As Integer, CveCategoria As Integer) As List(Of CatEquiposModel)
        Dim dt As DataTable
        Dim lst As New List(Of CatEquiposModel)
        Try
            dt = FindAll(CveEquipo, CveCategoria)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As CatEquiposModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveEquipo As Integer, CveCategoria As Integer) As CatEquiposModel
        Dim ObjM As New CatEquiposModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveEquipo, CveCategoria)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As CatEquiposModel
        Dim ObjModel As New CatEquiposModel
        ObjModel.CveEquipo = dr("CveEquipo")
        ObjModel.NomEquipo = dr("NomEquipo")
        ObjModel.Marca = dr("Marca")
        ObjModel.Serie = dr("Serie")
        ObjModel.Modelo = dr("Modelo")
        ObjModel.NoParte = dr("NoParte")
        ObjModel.CveCategoria = dr("CveCategoria")
        ObjModel.CveProveedor = dr("CveProveedor")
        ObjModel.FechaCompra = CDate(dr("FechaCompra")).ToString("dd/MM/yyyy")
        ObjModel.Costo = dr("Costo")
        ObjModel.CveMoneda = dr("CveMoneda")
        ObjModel.NomCategoria = dr("NomCategoria")
        ObjModel.NomProveedor = dr("NomProveedor")
        ObjModel.NomMoneda = dr("NomMoneda")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(CveEquipo As Integer, Valores As String, UsuAct As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=0")
            sb.Append(" DECLARE @CveEquipo int=" + CveEquipo.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiu_CatEquipos @Opcion,@CveEquipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
                    IsResult = True
                Else
                    Throw New Exception(dt(0)("Mensaje").ToString)
                End If
            End If

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function
    Public Function DeleteModel(CveEquipo As Integer) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=2")
            sb.Append(" DECLARE @CveEquipo int=" + CveEquipo.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct bigint=0")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiu_CatEquipos @Opcion,@CveEquipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
                    IsResult = True
                ElseIf dt(0)("Estatus").ToString = "-2" Then
                    Throw New Exception(New Mensajes().FindById("0", 0, 19).NomMensaje)
                Else
                    Throw New Exception(dt(0)("Mensaje").ToString)
                End If
            End If

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function

    Public Sub LlenaEquipos(DDLControl As Control, CveCategoria As Integer)
        Dim dt As DataTable

        Try
            dt = FindAll(0, CveCategoria)
            Call subControl_fill(DDLControl, dt, "CveEquipo", "Serie", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

End Class
