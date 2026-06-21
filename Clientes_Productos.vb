Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria

Public Class Clientes_Productos
    Public strError As String = ""
    Public Function GetSQL(CodCliente As String, CveProducto As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveProducto int=" + CveProducto.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Clientes_Productos @CodCliente,@CveProducto,@Estatus Output,@Mensaje Output")

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
    Public Function FindlstAll(CodCliente As String, CveProducto As Integer) As List(Of Clientes_ProductosModel)
        Dim dt As DataTable
        Dim lst As New List(Of Clientes_ProductosModel)
        Try
            dt = FindAll(CodCliente, CveProducto)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Clientes_ProductosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveProducto As Integer) As Clientes_ProductosModel
        Dim ObjM As New Clientes_ProductosModel
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
    Public Function FillModel(dr As DataRow) As Clientes_ProductosModel
        Dim ObjModel As New Clientes_ProductosModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.NomProducto = dr("NomProducto")
        ObjModel.CodProducto = dr("CodProducto")
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.Dependencias = dr("Dependencias")
        ObjModel.DependenciasM = dr("DependenciasM")


        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function
    Public Function SaveModel(CodCliente As String, CveProducto As Integer, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Opcion int = 0")
            sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
            sb.Append(" DECLARE @CveProducto int=" + CveProducto.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spiud_Clientes_Productos @Opcion,@CodCliente,@CveProducto,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
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
    Public Function DeleteModel(CodCliente As String, CveProducto As Integer, Valores As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Opcion int = 2")
            sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
            sb.Append(" DECLARE @CveProducto int=" + CveProducto.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=0")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spiud_Clientes_Productos @Opcion,@CodCliente,@CveProducto,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
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

End Class
