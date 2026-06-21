Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerC_PerfilN
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePerfilN As Int64, CodUsuario As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerC_PerfilNutricional @CvePerfilN,@CodUsuario,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePerfilN As Int64, CodUsuario As String) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(CvePerfilN, CodUsuario)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePerfilN As Int64, CodUsuario As String) As List(Of OptimizerC_PerfilNModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerC_PerfilNModel)
        Try
            dt = FindAll(CvePerfilN, CodUsuario)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerC_PerfilNModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePerfilN As Int64, CodUsuario As String) As OptimizerC_PerfilNModel
        Dim dt As DataTable
        Dim ObjM As OptimizerC_PerfilNModel
        Try
            dt = FindAll(CvePerfilN, CodUsuario)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As OptimizerC_PerfilNModel
        Dim ObjModel As New OptimizerC_PerfilNModel
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveModalidad = dr("CveModalidad")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.CveReferencia = dr("CveReferencia")
        ObjModel.Temperatura = dr("Temperatura")
        ObjModel.EspacioCorral = dr("EspacioCorral")
        ObjModel.RAC = dr("RAC")
        ObjModel.Conclusion = dr("Conclusion")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomModalidad = dr("NomModalidad")
        ObjModel.NomReferencia = dr("NomReferencia")

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

    Public Function SaveModel(CvePerfilN As Int64, Valores As String, ValoresE As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int=0")
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresE varchar(MAX)='" + ValoresE + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerC_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    Public Function DeleteModel(CvePerfilN As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Opcion int=2")
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)=''")
            sb.Append(" DECLARE @ValoresE varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct varchar(50)=''")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerC_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")


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