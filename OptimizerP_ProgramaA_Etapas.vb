Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerP_ProgramaA_Etapas
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As String
        Dim sb As New StringBuilder

        sb.Append(" SELECT ")
        sb.Append(" ISNULL(PP.CvePlan,0) as CvePlan,")
        sb.Append(" tbl.CveEtapa,")
        sb.Append(" Et.NomEtapa,")
        sb.Append(" CASE WHEN PP.CvePlan IS NULL THEN 'N' ELSE ISNULL(PP.Aplica,'N') END as Aplica,")
        sb.Append(" ISNULL(PP.Costo,0) as Costo,")
        sb.Append(" ISNULL(PP.EM,0) as EM,")
        sb.Append(" ISNULL(PP.SIDLYS,0) as SIDLYS,")
        sb.Append(" ISNULL(PP.DuracionMin,0) as DuracionMin,")
        sb.Append(" ISNULL(PP.DuracionMax,0) as DuracionMax,")
        sb.Append(" PP.FecAct,")
        sb.Append(" ISNULL(PP.UsuAct,'') as UsuAct,")
        sb.Append(" ISNULL(OG.NomOpcion,'') as NomAplica,")
        sb.Append(" ISNULL(Et.CodALLIX,'') as CodALLIX,")
        sb.Append(" ISNULL(U.NomUsuario,'') as NomUsuAct")
        sb.Append(" FROM CatOptimizerP_Etapas tbl")
        sb.Append(" LEFT JOIN OptimizerP_PlanA_Etapas PP ON PP.CveEtapa=tbl.CveEtapa AND PP.CvePlan=" + CvePlan.ToString)
        sb.Append(" INNER JOIN OptimizerP_PerfilN_Etapas Et ON Et.CvePerfilN=" + CvePerfil.ToString + " AND Et.CveEtapa=tbl.CveEtapa")
        sb.Append(" LEFT JOIN CatOpcionesGenerales OG ON OG.CveOpcion = PP.Aplica AND OG.Categoria=1")
        sb.Append(" LEFT JOIN Usuarios U ON U.CodUsuario = PP.UsuAct")
        sb.Append(" ORDER BY tbl.CveEtapa")

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
    Public Function FindlstAll(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As List(Of OptimizerP_ProgramaA_EtapasModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerP_ProgramaA_EtapasModel)
        Try
            dt = FindAll(CodCliente, CvePlan, CvePerfil)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerP_ProgramaA_EtapasModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CvePlan As Int64, CvePerfil As Int64) As OptimizerP_ProgramaA_EtapasModel
        Dim dt As DataTable
        Dim ObjM As OptimizerP_ProgramaA_EtapasModel
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
    Public Function FillModel(dr As DataRow) As OptimizerP_ProgramaA_EtapasModel
        Dim ObjModel As New OptimizerP_ProgramaA_EtapasModel
        Try
            ObjModel.CvePlan = dr("CvePlan")
            ObjModel.CveEtapa = dr("CveEtapa")
            ObjModel.NomEtapa = dr("NomEtapa")
            ObjModel.Aplica = dr("Aplica")
            ObjModel.Costo = dr("Costo")
            ObjModel.EM = dr("EM")
            ObjModel.SIDLYS = dr("SIDLYS")
            ObjModel.DuracionMin = dr("DuracionMin")
            ObjModel.DuracionMax = dr("DuracionMax")
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
