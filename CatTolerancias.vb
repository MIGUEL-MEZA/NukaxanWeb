Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class CatTolerancias
    Public strError As String = ""
    Public Function GetSQL(CveTipoProducto As Integer, CveFamProd As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveTipoProducto int=" + CveTipoProducto.ToString)
        sb.Append(" DECLARE @CveFamProd varchar(10)='" + CveFamProd + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nireo_Tolerancias @CveTipoProducto,@CveFamProd,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveTipoProducto As Integer, CveFamProd As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveTipoProducto, CveFamProd))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveTipoProducto As Integer, CveFamProd As String) As List(Of CatToleranciasModel)
        Dim dt As DataTable
        Dim lst As New List(Of CatToleranciasModel)
        Try
            dt = FindAll(CveTipoProducto, CveFamProd)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As CatToleranciasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveTipoProducto As Integer, CveFamProd As String) As CatToleranciasModel
        Dim ObjM As New CatToleranciasModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveTipoProducto, CveFamProd)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As CatToleranciasModel
        Dim ObjModel As New CatToleranciasModel
        ObjModel.CveTipoProducto = dr("CveTipoProducto")
        ObjModel.TipoProd = dr("TipoProd")
        ObjModel.CveFamProd = dr("CveFamProd")
        ObjModel.CveAna = dr("CveAna")
        ObjModel.NomFamProd = dr("NomFamProd")
        ObjModel.NomAna = dr("NomAna")
        ObjModel.Valor_Min = dr("Valor_Min")
        ObjModel.Valor_Max = dr("Valor_Max")
        ObjModel.TipoCalculo = dr("TipoCalculo")
        ObjModel.Operador = dr("Operador")
        ObjModel.Limite_Inf = dr("Limite_Inf")
        ObjModel.Limite_Sup = dr("Limite_Sup")

        Return ObjModel
    End Function
End Class
