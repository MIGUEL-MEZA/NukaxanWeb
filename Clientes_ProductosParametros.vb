Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria

Public Class Clientes_ProductosParametros
    Public strError As String = ""
    Public Function GetSQL(Opcion As Integer, CodCliente As String, CveProducto As Integer, CveParametro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @Opcion int=" + Opcion.ToString)
        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveProducto int=" + CveProducto.ToString)
        sb.Append(" DECLARE @CveParametro int=" + CveParametro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Clientes_ProductosParametros @Opcion,@CodCliente,@CveProducto,@CveParametro,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Opcion As Integer, CodCliente As String, CveProducto As Integer, CveParametro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(Opcion, CodCliente, CveProducto, CveParametro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Opcion As Integer, CodCliente As String, CveProducto As Integer, CveParametro As Integer) As List(Of Clientes_Productos_ParametrosModel)
        Dim dt As DataTable
        Dim lst As New List(Of Clientes_Productos_ParametrosModel)
        Try
            dt = FindAll(Opcion, CodCliente, CveProducto, CveParametro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Clientes_Productos_ParametrosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Opcion As Integer, CodCliente As String, CveProducto As Integer, CveParametro As Integer) As Clientes_Productos_ParametrosModel
        Dim ObjM As New Clientes_Productos_ParametrosModel
        Dim dt As DataTable
        Try
            dt = FindAll(Opcion, CodCliente, CveProducto, CveParametro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Clientes_Productos_ParametrosModel
        Dim ObjModel As New Clientes_Productos_ParametrosModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.ValorMin = dr("ValorMin")
        ObjModel.ValorMax = dr("ValorMax")
        ObjModel.ValorEsperado = dr("ValorEsperado")
        ObjModel.CodParametro = dr("CodParametro")
        ObjModel.NomParametro = dr("NomParametro")
        ObjModel.Dependencias = dr("Dependencias")
        ObjModel.Archivo = dr("Archivo")

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
            sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
            sb.Append(" DECLARE @CveProducto int=" + CveProducto.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spiud_Clientes_ProductosParametros @CodCliente,@CveProducto,@Valores,@UsuAct,@Estatus Output,@Mensaje Output")

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
