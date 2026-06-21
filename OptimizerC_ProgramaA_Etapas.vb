Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_ProgramaA_Etapas
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CodCliente As String, CvePlan As Int64) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CvePlan int=" + CvePlan.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerC_PlanAlimentacion_Etapas @CodCliente,@CvePlan,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CvePlan As Int64) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CodCliente, CvePlan)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CvePlan As Int64) As List(Of OptimizerC_ProgramaA_EtapasModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_ProgramaA_EtapasModel)
        Try
            dt = FindAll(CodCliente, CvePlan)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_ProgramaA_EtapasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CvePlan As Int64) As OptimizerC_ProgramaA_EtapasModel
        Dim dt As DataTable
        Dim ObjM As OptimizerC_ProgramaA_EtapasModel
        Try
            dt = FindAll(CodCliente, CvePlan)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_ProgramaA_EtapasModel
        Dim ObjModel As New OptimizerC_ProgramaA_EtapasModel
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.CveProducto = dr("CveProducto")
        ObjModel.Aplica = dr("Aplica")
        ObjModel.Costo = dr("Costo")
        ObjModel.CA = dr("CA")
        ObjModel.Ractopamina = dr("Ractopamina")
        ObjModel.EM = dr("EM")
        ObjModel.EN = dr("EN")
        ObjModel.SIDLYS = dr("SIDLYS")
        ObjModel.Presupuesto = dr("Presupuesto")
        'ObjModel.PesoFinal = dr("PesoFinal")
        ObjModel.DuracionMin = dr("DuracionMin")
        ObjModel.DuracionMax = dr("DuracionMax")
        ObjModel.NomAplica = dr("NomAplica")
        ObjModel.NomProducto = dr("NomProducto")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.IsEtapa = dr("IsEtapa")
        ObjModel.IsRactopamina = dr("IsRactopamina")
        ObjModel.IsEM = dr("IsEM")
        ObjModel.IsEN = dr("IsEN")
        ObjModel.IsSID = dr("IsSID")
        ObjModel.IsPresupuesto = dr("IsPresupuesto")
        'ObjModel.IsPesoFinal = dr("IsPesoFinal")
        ObjModel.IsDuracionMin = dr("IsDuracionMin")
        ObjModel.IsDuracionMax = dr("IsDuracionMax")
        ObjModel.CodALLIX = dr("CodALLIX")

        'Bitacora
        ObjModel.FecAct = If(IsDBNull(dr("FecAct")), "", CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm"))
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function


End Class