Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.OptimizerP_PerfilN

Public Class OptimizerG_PerfilN
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Function getSQL(CvePerfilN As Int64, CodUsuario As String) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerG_PerfilNutricional @CvePerfilN, @CodUsuario,@Estatus Output,@Mensaje Output")

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
    Public Function FindlstAll(CvePerfilN As Int64, CodUsuario As String) As List(Of OptimizerG_PerfilNModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerG_PerfilNModel)
        Try
            dt = FindAll(CvePerfilN, CodUsuario)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerG_PerfilNModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePerfilN As Int64, CodUsuario As String) As OptimizerG_PerfilNModel
        Dim dt As DataTable
        Dim ObjM As OptimizerG_PerfilNModel
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
    Public Function FillModel(dr As DataRow) As OptimizerG_PerfilNModel
        Dim ObjModel As New OptimizerG_PerfilNModel
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveModalidad = dr("CveModalidad")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.CveReferencia = dr("CveReferencia")
        ObjModel.Temperatura = dr("Temperatura")
        ObjModel.Humedad = dr("Humedad")
        ObjModel.ITH = dr("ITH")
        ObjModel.CveITH = dr("CveITH")
        ObjModel.ImgITH = dr("ImgITH")
        ObjModel.DesperdicioP = dr("DesperdicioP")
        ObjModel.DesperdicioC = dr("DesperdicioC")
        ObjModel.CveEstatusC = dr("CveEstatusC")
        ObjModel.CveTipoInstalacion = dr("CveTipoInstalacion")
        ObjModel.Conclusion = dr("Conclusion")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.NomModalidad = dr("NomModalidad")
        ObjModel.NomReferencia = dr("NomReferencia")
        ObjModel.NomITH = dr("NomITH")
        ObjModel.NomEstatusC = dr("NomEstatusC")
        ObjModel.NomTipoInstalacion = dr("NomTipoInstalacion")

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

            sb.Append(" EXEC spiud_OptimizerG_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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

            sb.Append(" EXEC spiud_OptimizerG_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")


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

    Public Function ActualizaEditable(Opcion As Integer, CvePerfilN As Int64, Valores As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Opcion int=" + Opcion.ToString)
            sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresE varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_OptimizerG_PerfilNutricional @Opcion,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")


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

    Public Function ConstruirModeloCaptura(Id As Int64, CodCliente As String) As List(Of PNCapturaModel)
        Dim ObjR As ResponseModel = JsonConvert.DeserializeObject(Of ResponseModel)(New OptimizerG_PerfilN_Resultado().FindById(Id).Response)
        Dim lstE As List(Of OptimizerG_PerfilN_EtapasModel) = New OptimizerG_PerfilN_Etapas().FindlstAll(CodCliente, Id)
        Dim lstVariables As List(Of OptimizerG_CatVariablesModel) = New OptimizerG_CatVariables().FindlstAll(0)


        Dim resultado As New List(Of PNCapturaModel)
        For Each variable In ObjR.Variables.Where(Function(v) v.MostrarCliente = "S").OrderBy(Function(v) v.Posicion)
            Dim infoVar = lstVariables.FirstOrDefault(Function(v) v.CveVariable = variable.NoVariable)
            Dim decimales As Integer = If(infoVar IsNot Nothing, infoVar.Decimales, 2)
            Dim EditarAjuste As String = If(infoVar IsNot Nothing, infoVar.EditarAjuste, "N")
            Dim CveCategoria As Integer = If(infoVar IsNot Nothing, infoVar.CveCategoria, 0)
            Dim NomCategoria As String = If(infoVar IsNot Nothing, infoVar.NomCategoria, "")
            Dim ReporteInterno As String = If(infoVar IsNot Nothing, infoVar.ReporteInterno, "N")
            Dim ReporteExterno As String = If(infoVar IsNot Nothing, infoVar.ReporteExterno, "N")
            Dim EnvioFlujo As String = If(infoVar IsNot Nothing, infoVar.EnvioFlujo, "N")

            Dim mostrar() As String = If(infoVar IsNot Nothing, infoVar.MostrarValores.Split(","), "")
            For Each etapa In lstE.Where(Function(e) e.Aplica = "S")
                Dim etapaData = variable.Etapas.FirstOrDefault(Function(x) x.Clave = etapa.CveEtapa)
                Dim valor As Double = 0
                If etapaData IsNot Nothing Then
                    Dim raw As String = etapaData.Valor.ToString().Trim()
                    If raw = "" Then
                        valor = 0
                    ElseIf raw.Equals("NaN", StringComparison.OrdinalIgnoreCase) _
                        OrElse raw.Equals("Infinity", StringComparison.OrdinalIgnoreCase) _
                        OrElse raw.Equals("-Infinity", StringComparison.OrdinalIgnoreCase) Then
                        valor = 0 ' o Nothing si quieres interpretar como vacío

                    ElseIf Double.TryParse(raw, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, valor) Then
                    Else
                        valor = 0
                    End If
                End If

                resultado.Add(New PNCapturaModel With {
                .Etapa = etapa.CveEtapa,
                .NombreEtapa = etapa.NomEtapa,
                .Variable = variable.NoVariable,
                .Descripcion = variable.Variable,
                .Decimales = decimales,
                .Referencia = valor,
                .Ajuste = valor,
                .Comentario = "",
                .Mostrar = mostrar(etapa.CveEtapa - 1),
                .CveCategoria = CveCategoria,
                .EditarAjuste = EditarAjuste,
                .ReporteInterno = ReporteInterno,
                .ReporteExterno = ReporteExterno,
                .EnvioFlujo = EnvioFlujo,
                .NomCategoria = NomCategoria
            })

            Next
        Next

        Return resultado
    End Function
End Class