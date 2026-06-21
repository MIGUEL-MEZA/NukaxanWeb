Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Usuario
    Public strError As String = ""
    Public Id As Int64 = 0
    Public CodUsuario As String = ""
    Public Folio As String = 0
    Public Function ValidaLogin(UserName As String, Password As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @UserName varchar(30)='" + UserName + "'")
            sb.Append(" DECLARE @Password varchar(30)='" + Password + "'")
            sb.Append(" DECLARE @Estatus int=1")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id bigint=0")
            sb.Append(" EXEC spp_ValidaLogin @UserName,@Password,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" And dt(0)("Id").ToString <> "0" Then
                    Id = dt(0)("Id")
                    CodUsuario = dt(0)("CodUsuario")
                    IsResult = True
                ElseIf dt(0)("Estatus").ToString = "0" And dt(0)("Id").ToString = "0" Then
                    Throw New Exception(New Mensajes().FindById("1", 2, 1).NomMensaje)
                ElseIf dt(0)("Estatus").ToString = "-1" Then
                    IsResult = False
                    Throw New Exception("Usuario Inactivo")
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
    Public Function GetSQL(Opcion As Integer, CveUsuario As Int64, CodUsuario As String, Optional CveTipo As String = "", Optional CveEstatus As Integer = 0) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @Opcion int = " + Opcion.ToString)
        sb.Append(" DECLARE @CveUsuario bigint = " + CveUsuario.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @CveTipo char(1) ='" + CveTipo + "'")
        sb.Append(" DECLARE @CveEstatus int = " + CveEstatus.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Usuarios @Opcion,@CveUsuario,@CodUsuario,@CveTipo,@CveEstatus,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Opcion As Integer, CveUsuario As Int64, CodUsuario As String, Optional CveTipo As String = "", Optional CveEstatus As Integer = 0) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(Opcion, CveUsuario, CodUsuario, CveTipo, CveEstatus))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Opcion As Integer, CveUsuario As Int64, CodUsuario As String, Optional CveTipo As String = "", Optional CveEstatus As Integer = 0) As List(Of UsuarioModel)
        Dim dt As DataTable
        Dim lst As New List(Of UsuarioModel)
        Try
            dt = FindAll(Opcion, CveUsuario, CodUsuario, CveTipo, CveEstatus)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As UsuarioModel = FillModel(Opcion, dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Opcion As Integer, CveUsuario As Int64, CodUsuario As String) As UsuarioModel
        Dim ObjM As New UsuarioModel
        Dim dt As DataTable
        Try
            dt = FindAll(Opcion, CveUsuario, CodUsuario, "", 0)
            If Not dt Is Nothing Then ObjM = FillModel(Opcion, dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(Opcion As Integer, dr As DataRow) As UsuarioModel
        Dim ObjModel As New UsuarioModel
        Select Case Opcion
            Case 0
                ObjModel.userId = dr("userId")
                ObjModel.CodUsuario = dr("CodUsuario")
                ObjModel.NomUsuario = dr("NomUsuario")
                ObjModel.Email = dr("Email")
                ObjModel.NomPuesto = dr("NomPuesto")
                ObjModel.NomUbicacion = dr("NomUbicacion")
                ObjModel.NomArea = dr("NomArea")
                ObjModel.CveTipo = dr("CveTipo")
                ObjModel.CveLenguaje = dr("CveLenguaje")
                ObjModel.CveRol = dr("CveRol")
                ObjModel.CveEstatus = dr("CveEstatus")
                ObjModel.TotalRelCte = dr("TotalRelCte")

            Case 1
                ObjModel.userId = dr("userId")
                ObjModel.CodUsuario = dr("CodUsuario")
                ObjModel.NomUsuario = dr("NomUsuario")
                ObjModel.Email = dr("Email")
                ObjModel.NomPuesto = dr("NomPuesto")
                ObjModel.NomUbicacion = dr("NomUbicacion")
                ObjModel.NomArea = dr("NomArea")
                ObjModel.NomLider = dr("NomLider")
                ObjModel.CveTipo = dr("CveTipo")
                ObjModel.CvePuesto = dr("CvePuesto")
                ObjModel.CveUbicacion = dr("CveUbicacion")
                ObjModel.CveArea = dr("CveArea")
                ObjModel.CveLider = dr("CveLider")
                ObjModel.CveLenguaje = dr("CveLenguaje")
                ObjModel.CveRol = dr("CveRol")
                ObjModel.SegUsuario = dr("SegUsuario")
                ObjModel.SegPassword = dr("SegPassword")
                ObjModel.NomTipo = dr("NomTipo")
                ObjModel.NomRol = dr("NomRol")
                ObjModel.Dependencias = dr("Dependencias")

                'Bitacora
                ObjModel.CveEstatus = dr("CveEstatus")
                ObjModel.NomEstatus = dr("NomEstatus")
                ObjModel.FecAlta = CDate(dr("FecAlta")).ToString("dd/MM/yyyy HH:mm")
                ObjModel.UsuAlta = dr("UsuAlta")
                ObjModel.NomUsuAlta = dr("NomUsuAlta")
                ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
                ObjModel.UsuAct = dr("UsuAct")
                ObjModel.NomUsuAct = dr("NomUsuAct")
        End Select

        Return ObjModel
    End Function

    Public Function SaveModel(Opcion As Integer, userId As Int64, Tipo As String, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=" + Opcion.ToString)
            sb.Append(" DECLARE @userId bigint=" + userId.ToString)
            sb.Append(" DECLARE @Tipo char(1)='" + Tipo + "'")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id varchar(50)=''")

            sb.Append(" EXEC spiud_Usuarios @Opcion,@userId,@Tipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    Public Function DeleteModel(userId As Int64, Tipo As String, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Try
            sb.Append(" DECLARE @Opcion int=2")
            sb.Append(" DECLARE @userId bigint=" + userId.ToString)
            sb.Append(" DECLARE @Tipo char(1)='" + Tipo + "'")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_Usuarios @Opcion,@userId,@Tipo,@Valores,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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

    Public Function Obten_NumClientesxPlataforma(CvePlataforma As Integer, CodUsuario As String) As Integer
        Dim numC As Integer = 0
        strError = ""
        Try
            numC = New Nukaxan_PerfilCliente().FindlstAll(CvePlataforma, CodUsuario, "").Count
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return numC
    End Function
End Class
