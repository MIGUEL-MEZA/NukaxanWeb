Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerP_ProgramaA
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlan bigint=" + CvePlan.ToString)
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerP_ProgramaA @CvePlan,@CvePerfilN,@CodUsuario,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CvePlan, CvePerfilN, CodUsuario)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As List(Of OptimizerP_ProgramaAModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerP_ProgramaAModel)
        Try
            dt = FindAll(CvePlan, CvePerfilN, CodUsuario)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerP_ProgramaAModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As OptimizerP_ProgramaAModel
        Dim dt As DataTable
        Dim ObjM As OptimizerP_ProgramaAModel
        Try
            dt = FindAll(CvePlan, CvePerfilN, CodUsuario)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerP_ProgramaAModel
        Dim ObjModel As New OptimizerP_ProgramaAModel
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.Folio = dr("Folio")
        ObjModel.FolioR = dr("FolioR")
        ObjModel.FolioRPN = dr("FolioRPN")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveModalidad = dr("CveModalidad")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.CveReferencia = dr("CveReferencia")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.EdadVenta = dr("EdadVenta")
        ObjModel.DiasPigmento = dr("DiasPigmento")
        ObjModel.PrecioVenta = dr("PrecioVenta")
        ObjModel.Desperdicio = dr("Desperdicio")
        ObjModel.Conclusion = dr("Conclusion")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.CodALLIXCte = dr("CodALLIXCte")
        ObjModel.NomModalidad = dr("NomModalidad")
        ObjModel.NomReferencia = dr("NomReferencia")
        ObjModel.NomParametro = dr("NomParametro")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAlta = CDate(dr("FecAlta")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAlta = dr("UsuAlta")
        ObjModel.NomUsuAlta = dr("NomUsuAlta")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(CvePlan As Int64, CvePerfilN As Int64, Valores As String, ValoresE As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=0")
            sb.Append(" DECLARE @CvePlan bigint=" + CvePlan.ToString)
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresE varchar(MAX)='" + ValoresE + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerP_ProgramaA @Opcion,@CvePlan,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    Folio = dt(0)("Id").ToString
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