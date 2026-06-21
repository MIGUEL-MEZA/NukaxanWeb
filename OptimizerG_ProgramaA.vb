Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class OptimizerG_ProgramaA
    Dim strSQLExe As String = ""
    Public strError As String = ""
    Public Folio As String = 0
    Public Class ResultadoPAModel
        Public Property CveEtapa As Integer
        Public Property NomEtapa As String
        Public Property Costo As Double
        Public Property EdadInicial As Double
        Public Property EdadFinal As Double
        Public Property Semanas As Double
        Public Property Dias As Double
        Public Property Mortalidad As Double
        Public Property NoAves As Double
        Public Property ConsumoAlimento As Double
        Public Property ConsumoAlimentoTotal As Double
        Public Property PesoHuevo As Double
        Public Property Produccion As Double
        Public Property MasaHuevo As Double
        Public Property ConversionAlimenticia As Double
        Public Property HuevoProducido As Double
    End Class
    Public Function getSQL(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlan bigint=" + CvePlan.ToString)
        sb.Append(" DECLARE @CvePerfilN bigint=" + CvePerfilN.ToString)
        sb.Append(" DECLARE @CodUsuario varchar(50)='" + CodUsuario + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_OptimizerG_ProgramaA @CvePlan,@CvePerfilN,@CodUsuario,@Estatus Output,@Mensaje Output")

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
    Public Function FindlstAll(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As List(Of OptimizerG_ProgramaAModel)
        Dim dt As DataTable
        Dim lst As New List(Of OptimizerG_ProgramaAModel)
        Try
            dt = FindAll(CvePlan, CvePerfilN, CodUsuario)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As OptimizerG_ProgramaAModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlan As Int64, CvePerfilN As Int64, CodUsuario As String) As OptimizerG_ProgramaAModel
        Dim dt As DataTable
        Dim ObjM As OptimizerG_ProgramaAModel
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
    Public Function FillModel(dr As DataRow) As OptimizerG_ProgramaAModel
        Dim ObjModel As New OptimizerG_ProgramaAModel
        ObjModel.CvePlan = dr("CvePlan")
        ObjModel.CvePerfilN = dr("CvePerfilN")
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveModalidad = dr("CveModalidad")
        ObjModel.Titulo = dr("Titulo")
        ObjModel.CveReferencia = dr("CveReferencia")
        'ObjModel.CveParametro = dr("CveParametro")
        ObjModel.TotalGallinas = dr("TotalGallinas")
        ObjModel.TotalPollitas = dr("TotalPollitas")
        ObjModel.PrecioVentaH = dr("PrecioVentaH")
        ObjModel.Conclusion = dr("Conclusion")
        ObjModel.NomCliente = dr("NomCliente")
        ObjModel.CodALLIXCte = dr("CodALLIXCte")
        ObjModel.NomModalidad = dr("NomModalidad")
        ObjModel.NomReferencia = dr("NomReferencia")
        'ObjModel.NomParametro = dr("NomParametro")

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

            sb.Append(" EXEC spiud_OptimizerG_ProgramaA @Opcion,@CvePlan,@CvePerfilN,@Valores,@ValoresE,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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

    Function ObtenResultadoPA(Id As Int64) As List(Of ResultadoPAModel)
        Dim lstRPA As New List(Of ResultadoPAModel)
        Try
            Dim ObjPA As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Id, 0, "")
            Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = New Optimizer_Presupuesto_Optimizado().FindById(3, Id)
            Dim ObjR As WSOptimizerG_ResponseModel = JsonConvert.DeserializeObject(Of WSOptimizerG_ResponseModel)(ObjM.Response)
            Dim lstEtapas As List(Of OptimizerG_ProgramaA_EtapasModel) = New OptimizerG_ProgramaA_Etapas().FindlstAll("", Id, ObjPA.CvePerfilN)

            ObjR.Data.ForEach(Sub(p)
                                  Dim NomEtapa As String = lstEtapas.Find(Function(x) x.CveEtapa = p.Identificador).NomEtapa
                                  Dim lst As New ResultadoPAModel With {
                                  .CveEtapa = p.Identificador, .NomEtapa = NomEtapa, .Costo = p.Costo, .EdadInicial = p.EdadInicial, .EdadFinal = p.EdadFinal,
                                  .Semanas = p.Semanas, .Dias = p.Dias, .Mortalidad = p.Mortalidad, .NoAves = p.NoAves, .ConsumoAlimento = p.ConsumoAlimento,
                                  .ConsumoAlimentoTotal = p.ConsumoAlimentoTotal, .PesoHuevo = p.PesoHuevo, .Produccion = p.Produccion,
                                  .MasaHuevo = p.MasaHuevo, .ConversionAlimenticia = p.ConversionAlimenticia, .HuevoProducido = p.HuevoProducido
                                  }
                                  lstRPA.Add(lst)
                              End Sub)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return lstRPA
    End Function

    Function ObtenTotales(Id As Int64) As List(Of String)
        Dim lstAll As New List(Of String)
        Try
            Dim ObjPA As OptimizerG_ProgramaAModel = New OptimizerG_ProgramaA().FindById(Id, 0, "")
            Dim ObjM As WSOptimizer_Presupuesto_OptimizadoModel = New Optimizer_Presupuesto_Optimizado().FindById(3, Id)
            Dim ObjR As WSOptimizerG_ResponseModel = JsonConvert.DeserializeObject(Of WSOptimizerG_ResponseModel)(ObjM.Response)

            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.costoPonderado.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.ConsumoTotalAlimento.ToString("N2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.CostoProducidoHuevo.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.ConversionAlimenticia.ToString("N2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.MasaTotalHuevo.ToString("N2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.CostoProgramaAlimentacion.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.ConsumoTotalAlimentoParvada.ToString("N2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.CostoProgramaParvada.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.MasaHuevoParvada.ToString("N2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.IngresoHuevoParvada.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.UtilidadAlimentacionParvada.ToString("C2"))
            lstAll.Add(ObjR.Resultado.analisisProductivoTotal.Roi.ToString("C2"))

            lstAll.Add(ObjR.Resultado.AnalisisProductivoCrianza.CostoPonderadoCrianza.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoCrianza.ConsumoAlimentoCrianza.ToString("N2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoCrianza.CostoProgramaCrianza.ToString("C2"))

            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.CostoPonderadoPostura.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.ConsumoAlimentoPostura.ToString("N2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.CostoProducidoPostura.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.ConversionAlimenticiaPostura.ToString("N2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.CostoProgramaPostura.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.MasaHuevoPostura.ToString("N2"))

            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.IngresoVentaHuevo.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.UtilidadAlimentacion.ToString("C2"))
            lstAll.Add(ObjR.Resultado.AnalisisProductivoPostura.roiPostura.ToString("C2"))

        Catch ex As Exception

        End Try
        Return lstAll
    End Function

End Class