Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Nireo_Muestreos
    Public strError As String = ""
    Public Function GetSQL(Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @Filtros varchar(MAX) ='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Muestras @Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Filtros As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(Filtros))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Filtros As String) As List(Of Nireo_MuestreosModel)
        Dim dt As DataTable
        Dim lst As New List(Of Nireo_MuestreosModel)
        Try
            dt = FindAll(Filtros)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Nireo_MuestreosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Filtros As String) As Nireo_MuestreosModel
        Dim ObjM As New Nireo_MuestreosModel
        Dim dt As DataTable
        Try
            dt = FindAll(Filtros)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Nireo_MuestreosModel
        Dim ObjModel As New Nireo_MuestreosModel
        ObjModel.CveMuestra = dr("CveMuestra")
        ObjModel.CveSesion = dr("CveSesion")
        ObjModel.CveEquipo = dr("CveEquipo")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.Identificacion = dr("Identificacion")
        ObjModel.FecMuestreo = dr("FecMuestreo")
        ObjModel.NumMuestreos = dr("NumMuestreos")
        ObjModel.Referencia = dr("Referencia")
        ObjModel.Lote = dr("Lote")
        ObjModel.CveOrigen = dr("CveOrigen")
        ObjModel.CveProveedor = dr("CveProveedor")
        ObjModel.Nota = dr("Nota")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.Serie = dr("Serie")
        ObjModel.NomProducto = dr("NomProducto")
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.NomOrigen = dr("NomOrigen")
        ObjModel.NomProveedor = dr("NomProveedor")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAlta = dr("FecAlta")
        ObjModel.UsuAlta = dr("UsuAlta")
        ObjModel.NomUsuAlta = dr("NomUsuAlta")

        Return ObjModel
    End Function


End Class
