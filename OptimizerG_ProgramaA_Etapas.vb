Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerG_ProgramaA_Etapas
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CvePlan int=" + CvePlan.ToString)
        sb.Append(" DECLARE @CvePerfil int=" + CvePerfil.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerG_ProgramaA_Etapas @CodCliente,@CvePlan,@CvePerfil,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CodCliente, CvePlan, CvePerfil)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As List(Of OptimizerG_ProgramaA_EtapasModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerG_ProgramaA_EtapasModel)
        Try
            dt = FindAll(CodCliente, CvePlan, CvePerfil)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerG_ProgramaA_EtapasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As OptimizerG_ProgramaA_EtapasModel
        Dim dt As DataTable
        Dim ObjM As OptimizerG_ProgramaA_EtapasModel
        Try
            dt = FindAll(CodCliente, CvePlan, CvePerfil)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerG_ProgramaA_EtapasModel
        Dim ObjModel As New OptimizerG_ProgramaA_EtapasModel
        Try
            ObjModel.CvePlan = dr("CvePlan")
            ObjModel.CveEtapa = dr("CveEtapa")
            ObjModel.NomEtapa = dr("NomEtapa")
            ObjModel.Aplica = dr("Aplica")
            ObjModel.Fija = dr("Fija")
            ObjModel.CveTipo = dr("CveTipo")
            ObjModel.Costo = dr("Costo")
            ObjModel.EdadIni = dr("EdadIni")
            ObjModel.EdadFin = dr("EdadFin")
            ObjModel.Mortalidad = dr("Mortalidad")
            ObjModel.ConsumoAlimento = dr("ConsumoAlimento")
            ObjModel.PesoHuevo = dr("PesoHuevo")
            ObjModel.Produccion = dr("Produccion")
            ObjModel.NomAplica = dr("NomAplica")
            ObjModel.CodALLIX = dr("CodALLIX")

            'Bitacora
            ObjModel.FecAct = If(IsDBNull(dr("FecAct")), "", CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm"))
            ObjModel.UsuAct = dr("UsuAct")
            ObjModel.NomUsuAct = dr("NomUsuAct")
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return ObjModel
    End Function


End Class