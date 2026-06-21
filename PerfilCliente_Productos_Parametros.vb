Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class PerfilCliente_Productos_Parametros
    Public strError As String = ""
    Public Function GetSQL(CodCliente As String, CveProducto As Integer, CveParametro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveProducto int =" + CveProducto.ToString)
        sb.Append(" DECLARE @CveParametro int =" + CveParametro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_PerfilCliente_Productos_Parametros @CodCliente,@CveProducto,@CveParametro,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveProducto As Integer, CveParametro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveProducto, CveParametro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveProducto As Integer, CveParametro As Integer) As List(Of PerfilCliente_Productos_ParametrosModel)
        Dim dt As DataTable
        Dim lst As New List(Of PerfilCliente_Productos_ParametrosModel)
        Try
            dt = FindAll(CodCliente, CveProducto, CveParametro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PerfilCliente_Productos_ParametrosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveProducto As Integer, CveParametro As Integer) As PerfilCliente_Productos_ParametrosModel
        Dim ObjM As New PerfilCliente_Productos_ParametrosModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveProducto, CveParametro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As PerfilCliente_Productos_ParametrosModel
        Dim ObjModel As New PerfilCliente_Productos_ParametrosModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.ValorMin = dr("ValorMin")
        ObjModel.ValorMax = dr("ValorMax")
        ObjModel.ValorEsperado = dr("ValorEsperado")
        ObjModel.CodParametro = dr("CodParametro")
        ObjModel.CodALLIXPa = dr("CodALLIXPa")
        ObjModel.NomParametro = dr("NomParametro")
        ObjModel.CveTipoP = dr("CveTipoP")
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.NomProducto = dr("NomProducto")
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
