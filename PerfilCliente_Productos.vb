Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class PerfilCliente_Productos
    Public strError As String = ""
    Public Function GetSQL(CodCliente As String, CveProducto As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(25) ='" + CodCliente + "'")
        sb.Append(" DECLARE @CveProducto integer=" + CveProducto.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_PerfilCliente_Productos @CodCliente,@CveProducto,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveProducto As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveProducto))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveProducto As Integer) As List(Of PerfilCliente_ProductosModel)
        Dim dt As DataTable
        Dim lst As New List(Of PerfilCliente_ProductosModel)
        Try
            dt = FindAll(CodCliente, CveProducto)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PerfilCliente_ProductosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveProducto As Integer) As PerfilCliente_ProductosModel
        Dim ObjM As New PerfilCliente_ProductosModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveProducto)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As PerfilCliente_ProductosModel
        Dim ObjModel As New PerfilCliente_ProductosModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.CodProducto = dr("CodProducto")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.NomProducto = dr("NomProducto")
        ObjModel.CveCategoriaP = dr("CveCategoriaP")

        ObjModel.CodCategoriaP = dr("CodCategoriaP")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.CveTipoP = dr("CveTipoP")
        ObjModel.NomTipoP = dr("NomTipoP")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function
End Class
