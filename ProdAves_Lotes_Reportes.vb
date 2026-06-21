Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_Lotes_Reportes
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL_ResumenLote(CodCliente As String, Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @Filtros varchar(MAX)='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Resumen_Lote @CodCliente,@Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll_ResumenLote(CodCliente As String, Filtros As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL_ResumenLote(CodCliente, Filtros))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll_ResumenLote(CodCliente As String, Filtros As String) As List(Of ProdAves_Lotes_Reportes_LoteModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_Lotes_Reportes_LoteModel)
        Try
            dt = FindAll_ResumenLote(CodCliente, Filtros)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_Lotes_Reportes_LoteModel = FillModel_ResumenLote(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById_ResumenLote(CodCliente As String, Filtros As String) As ProdAves_Lotes_Reportes_LoteModel
        Dim ObjM As New ProdAves_Lotes_Reportes_LoteModel
        Dim dt As DataTable
        Try
            dt = FindAll_ResumenLote(CodCliente, Filtros)
            If Not dt Is Nothing Then ObjM = FillModel_ResumenLote(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel_ResumenLote(dr As DataRow) As ProdAves_Lotes_Reportes_LoteModel
        Dim ObjModel As New ProdAves_Lotes_Reportes_LoteModel
        ObjModel.CodCliente = dr("CodCliente")
        ObjModel.CveLote = dr("CveLote")
        ObjModel.Edad = dr("Edad")
        ObjModel.CodLote = dr("CodLote")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.CveGranja = dr("CveGranja")
        ObjModel.AvesMuertas = dr("AvesMuertas")
        ObjModel.AjusteAves = dr("AjusteAves")
        ObjModel.AvesMuertasAc = dr("AvesMuertasAc")
        ObjModel.AvesInicial = dr("AvesInicial")
        ObjModel.AvesFinal = dr("AvesFinal")
        ObjModel.Mortalidad = dr("Mortalidad")
        ObjModel.MortalidadAc = dr("MortalidadAc")
        ObjModel.Supervivencia = dr("Supervivencia")
        ObjModel.AlimentoServido = dr("AlimentoServido")
        ObjModel.ConsumoAlimAD = dr("ConsumoAlimAD")
        ObjModel.ConsumoAlimAc = dr("ConsumoAlimAc")
        ObjModel.AguaServida = dr("AguaServida")
        ObjModel.ConsumoAguaAD = dr("ConsumoAguaAD")
        ObjModel.ConsumoAguaAc = dr("ConsumoAguaAc")
        ObjModel.PesoAve = dr("PesoAve")
        ObjModel.GSP = dr("GSP")
        ObjModel.CA = dr("CA")
        ObjModel.UniforAve = dr("UniforAve")
        ObjModel.CVAve = dr("CVAve")
        ObjModel.PesoHuevo = dr("PesoHuevo")
        ObjModel.TotalHuevos = dr("TotalHuevos")
        ObjModel.ProduccionAD = dr("ProduccionAD")
        ObjModel.ProduccionAA = dr("ProduccionAA")
        ObjModel.HuevoAcAD = dr("HuevoAcAD")
        ObjModel.HuevoAcAA = dr("HuevoAcAA")
        ObjModel.MasaHuevoAD = dr("MasaHuevoAD")
        ObjModel.MasaHuevoAcAD = dr("MasaHuevoAcAD")
        ObjModel.MasaHuevoAA = dr("MasaHuevoAA")
        ObjModel.MasaHuevoAcAA = dr("MasaHuevoAcAA")

        'Estandares
        ObjModel.Est_MortalidadAc = If(dr("Est_MortalidadAc") = "", 0, dr("Est_MortalidadAc"))
        ObjModel.Est_PesoCorporal = If(dr("Est_PesoCorporal") = "", 0, dr("Est_PesoCorporal"))
        ObjModel.Est_ConsumoAlimAD = If(dr("Est_ConsumoAlimAD") = "", 0, dr("Est_ConsumoAlimAD"))
        ObjModel.Est_ConsumoAguaAD = If(dr("Est_ConsumoAguaAD") = "", 0, dr("Est_ConsumoAguaAD"))
        ObjModel.Est_ProduccionAD = If(dr("Est_ProduccionAD") = "", 0, dr("Est_ProduccionAD"))
        ObjModel.Est_HuevoAcAD = If(dr("Est_HuevoAcAD") = "", 0, dr("Est_HuevoAcAD"))
        ObjModel.Est_HuevoAcAA = If(dr("Est_HuevoAcAA") = "", 0, dr("Est_HuevoAcAA"))
        ObjModel.Est_MasaHuevoAcAA = If(dr("Est_MasaHuevoAcAA") = "", 0, dr("Est_MasaHuevoAcAA"))
        ObjModel.Est_PesoHuevo = If(dr("Est_PesoHuevo") = "", 0, dr("Est_PesoHuevo"))

        Return ObjModel
    End Function
    Public Function GetSQL_ResumenCaseta(CodCliente As String, Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @Filtros varchar(MAX)='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_Resumen_Caseta @CodCliente,@Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function GetDatos_ResumenCaseta(CodCliente As String, Filtros As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL_ResumenCaseta(CodCliente, Filtros))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function


    Public Function GetSQL_ResumenHuevoLote(CodCliente As String, Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @Filtros varchar(MAX)='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_ResumenHuevos_Lote @CodCliente,@Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function GetDatos_ResumenHuevoLote(CodCliente As String, Filtros As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL_ResumenHuevoLote(CodCliente, Filtros))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function GetSQL_ResumenHuevoCaseta(CodCliente As String, Filtros As String) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CodCliente varchar(50)='" + CodCliente + "'")
        sb.Append(" DECLARE @Filtros varchar(MAX)='" + Filtros + "'")
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_Lotes_ResumenHuevos_Caseta @CodCliente,@Filtros,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function GetDatos_ResumenHuevoCaseta(CodCliente As String, Filtros As String) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL_ResumenHuevoCaseta(CodCliente, Filtros))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function


End Class
