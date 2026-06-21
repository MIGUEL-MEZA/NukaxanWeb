Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_RegistroSemana
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveLoteR As Integer, Edad As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveLoteR int=" + CveLoteR.ToString)
        sb.Append(" DECLARE @Edad int=" + Edad.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Registro_Semana @CodCliente,@CveLote,@CveLoteR,@Edad,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, Edad As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveLoteR, Edad))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, Edad As Integer) As List(Of ProdAves_Lotes_RegistroSemanaModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_RegistroSemanaModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, Edad)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_RegistroSemanaModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveLoteR As Integer, Edad As Integer) As ProdAves_Lotes_RegistroSemanaModel
        Dim ObjM As New ProdAves_Lotes_RegistroSemanaModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, Edad)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_Lotes_RegistroSemanaModel
        Dim ObjModel As New ProdAves_Lotes_RegistroSemanaModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveLoteR = dr("CveLoteR")
        ObjModel.Edad = dr("Edad")
        ObjModel.CveCiclo = dr("CveCiclo")
        ObjModel.AvesMuertas = dr("AvesMuertas")
        ObjModel.AjusteAves = dr("AjusteAves")
        ObjModel.PesoAve = dr("PesoAve")
        ObjModel.UniforAve = dr("UniforAve")
        ObjModel.CVAve = dr("CVAve")
        ObjModel.AlimentoServido = dr("AlimentoServido")
        ObjModel.AguaServida = dr("AguaServida")
        ObjModel.PesoHuevo = dr("PesoHuevo")
        ObjModel.UniforHuevo = dr("UniforHuevo")
        ObjModel.TotalHuevos = dr("TotalHuevos")
        ObjModel.NomCaseta = dr("NomCaseta")
        ObjModel.NomCiclo = dr("NomCiclo")
        ObjModel.CveFase = dr("CveFase")
        ObjModel.NomFase = dr("NomFase")
        ObjModel.NomAlimento = dr("NomAlimento")
        ObjModel.CostoAlimento = dr("CostoAlimento")
        ObjModel.AvesAlojadas = dr("AvesAlojadas")
        ObjModel.AvesInicial = dr("AvesInicial")
        ObjModel.AvesFinal = dr("AvesFinal")
        ObjModel.Mortalidad = dr("Mortalidad")
        ObjModel.MortalidadAc = dr("MortalidadAc")
        ObjModel.Supervivencia = dr("Supervivencia")
        ObjModel.ConsumoAlimAD = dr("ConsumoAlimAD")
        ObjModel.ConsumoAlimAc = dr("ConsumoAlimAc")
        ObjModel.ConsumoAgua = dr("ConsumoAgua")
        ObjModel.ConsumoAguaAc = dr("ConsumoAguaAc")
        ObjModel.GananciaPeso = dr("GananciaPeso")
        ObjModel.HuevoAcAD = dr("HuevoAcAD")
        ObjModel.HuevosAcAA = dr("HuevosAcAA")
        ObjModel.ProduccionAD = dr("ProduccionAD")
        ObjModel.ProduccionAA = dr("ProduccionAA")
        ObjModel.MasaHuevoAD = dr("MasaHuevoAD")
        ObjModel.MasaHuevoAc = dr("MasaHuevoAc")
        ObjModel.MasaHuevoAA = dr("MasaHuevoAA")
        ObjModel.ConversionAlimAD = dr("ConversionAlimAD")
        ObjModel.ConversionAlimAA = dr("ConversionAlimAA")
        ObjModel.CostoProdHuevo = dr("CostoProdHuevo")
        ObjModel.UtilidadProdHuevo = dr("UtilidadProdHuevo")

        'Bitacora
        ObjModel.CveEstatus = dr("CveEstatus")
        ObjModel.NomEstatus = dr("NomEstatus")
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

End Class
