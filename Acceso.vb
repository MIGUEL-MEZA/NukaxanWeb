Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Acceso
    Dim strSQLExe As String = ""
    Public strError As String = ""

    '--PLATAFORMAS
    Public Function getSQLPlataformas(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @Rol int=" + Rol.ToString)
        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CvePlataformaP int=" + CvePlataformaP.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Acceso_Plataformas @Rol,@CvePlataforma,@CvePlataformaP,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAllPlataformas(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQLPlataformas(Rol, CvePlataforma, CvePlataformaP)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAllPlataformas(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As List(Of PlataformaModel)
        Dim dt As DataTable
        Dim lst As New List(Of PlataformaModel)
        Try
            dt = FindAllPlataformas(Rol, CvePlataforma, CvePlataformaP)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PlataformaModel = FillModelPlataformas(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindByIdPlataformas(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As PlataformaModel
        Dim dt As DataTable
        Dim ObjM As PlataformaModel
        Try
            dt = FindAllPlataformas(Rol, CvePlataforma, CvePlataformaP)
            If Not dt Is Nothing Then ObjM = FillModelPlataformas(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModelPlataformas(dr As DataRow) As PlataformaModel
        Dim ObjModel As New PlataformaModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CvePlataformaP = dr("CvePlataformaP")
        ObjModel.NomPlataforma = dr("NomPlataforma")
        ObjModel.Descripcion = dr("Descripcion")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.Url = dr("Url")
        ObjModel.ImgPlataforma = dr("ImgPlataforma")
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.Habilitada = dr("Habilitada")

        Return ObjModel
    End Function

    '--CLIENTES
    Public Function getSQLClientes(CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @CveEstatus int=" + CveEstatus.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Acceso_Clientes @CvePlataforma,@CodUsuario,@CveEstatus,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAllClientes(CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQLClientes(CvePlataforma, CodUsuario, CveEstatus)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAllClientes(CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer) As List(Of ClientesModel)
        Dim dt As DataTable
        Dim lst As New List(Of ClientesModel)
        Try
            dt = FindAllClientes(CvePlataforma, CodUsuario, CveEstatus)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ClientesModel = FillModelClientes(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindByIdClientes(CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer) As ClientesModel
        Dim dt As DataTable
        Dim ObjM As ClientesModel
        Try
            dt = FindAllClientes(CvePlataforma, CodUsuario, CveEstatus)
            If Not dt Is Nothing Then ObjM = FillModelClientes(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModelClientes(dr As DataRow) As ClientesModel
        Dim ObjModel As New ClientesModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomClienteA = dr("NomClienteA")
        ObjModel.NomClienteR = dr("NomClienteR")

        Return ObjModel
    End Function

    Public Sub LlenaClientes(DDLControl As Control, CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer)
        Dim dt As DataTable

        Try
            dt = FindAllClientes(CvePlataforma, CodUsuario, CveEstatus)
            Call subControl_fill(DDLControl, dt, "CodCliente", "NomClienteR", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub


    Public Function ClienteEditable(CvePlataforma As Integer, CodUsuario As String, CveEstatus As Integer) As Boolean
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Try
            dt = FindAllClientes(CvePlataforma, CodUsuario, CveEstatus)
            If dt.Rows.Count > 1 Then IsResult = True

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return IsResult
    End Function

End Class