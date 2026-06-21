Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Optimizer_Presupuesto_Optimizado
    Public strError As String = ""
    Public Function GetSQL(Opcion As Integer, CvePlan As Int64) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @Opcion  int=" + Opcion.ToString)
        sb.Append(" DECLARE @CvePlan bigint=" + CvePlan.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Optimizer_Presupuesto_Optimizado @Opcion,@CvePlan,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Opcion As Integer, CvePlan As Int64) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(Opcion, CvePlan))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Opcion As Integer, CvePlan As Int64) As List(Of WSOptimizer_Presupuesto_OptimizadoModel)
        Dim dt As DataTable
        Dim lst As New List(Of WSOptimizer_Presupuesto_OptimizadoModel)
        Try
            dt = FindAll(Opcion, CvePlan)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Opcion As Integer, CvePlan As Int64) As WSOptimizer_Presupuesto_OptimizadoModel
        Dim ObjM As New WSOptimizer_Presupuesto_OptimizadoModel
        Dim dt As DataTable
        Try
            dt = FindAll(Opcion, CvePlan)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As WSOptimizer_Presupuesto_OptimizadoModel
        Dim ObjModel As New WSOptimizer_Presupuesto_OptimizadoModel
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.Request = dr("Request")
        ObjModel.Response = dr("Response")

        Return ObjModel
    End Function
End Class
