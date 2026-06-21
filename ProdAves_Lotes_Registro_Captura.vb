Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_Registro_Captura
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @CveLote int=" + CveLote.ToString)
        sb.Append(" DECLARE @CveLoteR int=" + CveLoteR.ToString)
        sb.Append(" DECLARE @CveRegistro int=" + CveRegistro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Registro_Captura @CodCliente,@CveLote,@CveLoteR,@CveRegistro,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CodCliente, CveLote, CveLoteR, CveRegistro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer) As List(Of ProdAves_Lotes_Registro_CapturaModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_Registro_CapturaModel)
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, CveRegistro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_Registro_CapturaModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer) As ProdAves_Lotes_Registro_CapturaModel
        Dim ObjM As New ProdAves_Lotes_Registro_CapturaModel
        Dim dt As DataTable
        Try
            dt = FindAll(CodCliente, CveLote, CveLoteR, CveRegistro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_Lotes_Registro_CapturaModel
        Dim ObjModel As New ProdAves_Lotes_Registro_CapturaModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.CveLoteR = dr("CveLoteR")
        ObjModel.CveRegistro = dr("CveRegistro")
        ObjModel.FecCaptura = dr("FecCaptura")
        ObjModel.Edad = dr("Edad")
        ObjModel.NumDia = dr("NumDia")
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
        ObjModel.HuevoAD = dr("HuevoAD")
        ObjModel.HuevoAA = dr("HuevoAA")
        ObjModel.HuevoAcAD = dr("HuevoAcAD")
        ObjModel.HuevoAcAA = dr("HuevoAcAA")
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
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(Opcion As Integer, Valores As String, ValoresH As String, UsuAct As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =" + Opcion.ToString)
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresH varchar(MAX)='" + ValoresH + "'")
            sb.Append(" DECLARE @UsuAct varchar(50)='" + UsuAct + "'")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lote_Registro_Captura @Opcion,@Valores,@ValoresH,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")

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
    Public Function DeleteModel(Valores As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False
        Folio = "0"
        Try
            sb.Append(" DECLARE @Opcion int =2")
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @ValoresH varchar(MAX)=''")
            sb.Append(" DECLARE @UsuAct varchar(50)=''")
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")
            sb.Append(" DECLARE @Id int=0")

            sb.Append(" EXEC spiud_ProdAves_Lote_Registro_Captura @Opcion,@Valores,@ValoresH,@UsuAct,@Estatus Output,@Mensaje Output,@Id Output")


            dt = execQuery(sb.ToString)
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt(0)("Estatus").ToString = "0" Then
                    IsResult = True
                ElseIf dt(0)("Estatus").ToString = "-2" Then
                    Throw New Exception(New Mensajes().FindById("0", 0, 19).NomMensaje)
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

    Public Function Calculos(CodCliente As String, CveLote As Integer, CveLoteR As Integer, CveRegistro As Integer) As Boolean
        Dim IsResult As Boolean = False
        Try
            Dim ObjR As ProdAves_Lotes_RecepcionModel = New ProdAves_Lotes_Recepcion().FindById(
                CodCliente, CveLote, CveLoteR)
            Dim ObjCaptura As ProdAves_Lotes_Registro_CapturaModel = New ProdAves_Lotes_Registro_Captura().FindById(
                CodCliente, CveLote, CveLoteR, CveRegistro)

            Dim ObjCapturaAnt As ProdAves_Lotes_Registro_CapturaModel = If(CveRegistro = 1, Nothing, New ProdAves_Lotes_Registro_Captura().FindById(
                CodCliente, CveLote, CveLoteR, CveRegistro - 1))


            ObjCaptura.AvesInicial = If(CveRegistro = 1, ObjCaptura.AvesAlojadas, ObjCapturaAnt.AvesFinal)
            ObjCaptura.AvesFinal = ObjCaptura.AvesInicial - ObjCaptura.AvesMuertas - ObjCaptura.AjusteAves
            ObjCaptura.Mortalidad = Math.Round((ObjCaptura.AvesInicial / ObjCaptura.AvesMuertas) * 100, 2)
            ObjCaptura.MortalidadAc = If(CveRegistro = 1, ObjCaptura.Mortalidad, ObjCaptura.Mortalidad + ObjCapturaAnt.MortalidadAc)
            ObjCaptura.Supervivencia = If(CveRegistro = 1, 100 - ObjCaptura.Mortalidad, ObjCapturaAnt.Supervivencia - ObjCaptura.Mortalidad)
            'ObjCaptura.ConsumoAlimAD = If(ObjCaptura.AlimentoServido = 0, 0, Math.Round((ObjCaptura.AlimentoServido / ObjCaptura.AvesInicial) * 1000, 2))
            'ObjCaptura.ConsumoAlimAc = If(CveRegistro = 1, ObjCaptura.ConsumoAlimAD, ObjCaptura.ConsumoAlimAD + ObjCapturaAnt.ConsumoAlimAc)
            'ObjCaptura.ConsumoAgua = If(ObjCaptura.AlimentoServido = 0, 0,Math.Round((ObjCaptura.AguaServida * 1000) / ObjCaptura.AvesInicial, 2))
            'ObjCaptura.ConsumoAguaAc = If(CveRegistro = 1, ObjCaptura.ConsumoAgua, ObjCaptura.ConsumoAgua + ObjCapturaAnt.ConsumoAguaAc)
            ObjCaptura.ProduccionAD = Math.Round((ObjCaptura.TotalHuevos / ObjCaptura.AvesInicial) * 100, 2)
            ObjCaptura.ProduccionAA = Math.Round((ObjCaptura.TotalHuevos / ObjCaptura.AvesAlojadas) * 100, 2)
            'ObjCaptura.HuevoAD = Math.Round((ObjCaptura.TotalHuevos / ObjCaptura.AvesInicial), 2)
            'ObjCaptura.HuevoAA = Math.Round((ObjCaptura.TotalHuevos / ObjCaptura.AvesAlojadas), 2)
            'ObjCaptura.HuevoAcAD = If(CveRegistro = 1, ObjCaptura.HuevoAD, ObjCaptura.HuevoAD + ObjCapturaAnt.HuevoAcAD)
            'ObjCaptura.HuevoAcAA = If(CveRegistro = 1, ObjCaptura.HuevoAA, ObjCaptura.HuevoAA + ObjCapturaAnt.HuevoAcAA)
            'ObjCaptura.MasaHuevoAD = Math.Round((ObjCaptura.ProduccionAD * ObjCaptura.PesoHuevo) / 100, 2)
            'ObjCaptura.MasaHuevoAc = ObjCaptura.MasaHuevoAD + ObjCapturaAnt.MasaHuevoAc
            'ObjCaptura.MasaHuevoAA = Math.Round((ObjCaptura.ProduccionAA * ObjCaptura.PesoHuevo) / 100, 2)


            IsResult = True
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try
        Return IsResult
    End Function

End Class
