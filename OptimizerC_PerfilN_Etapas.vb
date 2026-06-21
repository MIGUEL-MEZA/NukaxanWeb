Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_PerfilN_Etapas
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CodCliente As String, CvePerfilN As Int64) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerC_PerfilNutricional_Etapas @CodCliente,@CvePerfilN,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CvePerfilN As Int64) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CodCliente, CvePerfilN)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CvePerfilN As Int64) As List(Of OptimizerC_PerfilN_EtapasModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_PerfilN_EtapasModel)
        Try
            dt = FindAll(CodCliente, CvePerfilN)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_PerfilN_EtapasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CvePerfilN As Int64) As OptimizerC_PerfilN_EtapasModel
        Dim dt As DataTable
        Dim ObjM As OptimizerC_PerfilN_EtapasModel
        Try
            dt = FindAll(CodCliente, CvePerfilN)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_PerfilN_EtapasModel
        Dim ObjModel As New OptimizerC_PerfilN_EtapasModel
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.NomEtapa = dr("NomEtapa")
        ObjModel.CodALLIX = dr("CodALLIX")
        ObjModel.IsRAC = dr("IsRAC")
        ObjModel.Aplica = dr("Aplica")
        ObjModel.PesoIni = dr("PesoIni")
        ObjModel.PesoFin = dr("PesoFin")
        ObjModel.ENAlimento = dr("ENAlimento")
        ObjModel.PorcGDP = dr("AjusteGDP")

        'Bitacora
        ObjModel.FecAct = If(IsDBNull(dr("FecAct")), "", CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm"))
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function


End Class