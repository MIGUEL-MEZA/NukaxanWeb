Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class ProdAves_CatGuias_Estandares
    Public strError As String = ""
    Public Folio As String = ""
    Public Function GetSQL(CveGuia As Integer, CveEtapa As Integer, Edad As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveGuia int=" + CveGuia.ToString)
        sb.Append(" DECLARE @CveEtapa int=" + CveEtapa.ToString)
        sb.Append(" DECLARE @Edad int=" + Edad.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_ProdAves_CatGuias_Estandares @CveGuia,@CveEtapa,@Edad,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveGuia As Integer, CveEtapa As Integer, Edad As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveGuia, CveEtapa, Edad))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveGuia As Integer, CveEtapa As Integer, Edad As Integer) As List(Of ProdAves_CatGuias_EstandaresModel)
        Dim dt As DataTable
        Dim lst As New List(Of ProdAves_CatGuias_EstandaresModel)
        Try
            dt = FindAll(CveGuia, CveEtapa, Edad)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ProdAves_CatGuias_EstandaresModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveGuia As Integer, CveEtapa As Integer, Edad As Integer) As ProdAves_CatGuias_EstandaresModel
        Dim ObjM As New ProdAves_CatGuias_EstandaresModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveGuia, CveEtapa, Edad)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ProdAves_CatGuias_EstandaresModel
        Dim ObjModel As New ProdAves_CatGuias_EstandaresModel
        ObjModel.CveGuia = dr("CveGuia")
        ObjModel.CveEtapa = dr("CveEtapa")
        ObjModel.Edad = dr("Edad")
        ObjModel.Est_MortalidadAc = dr("Est_MortalidadAc")
        ObjModel.Est_PesoCorporal = dr("Est_PesoCorporal")
        ObjModel.Est_ConsumoAlimAD = dr("Est_ConsumoAlimAD")
        ObjModel.Est_ConsumoAguaAD = dr("Est_ConsumoAguaAD")
        ObjModel.Est_ProduccionAD = dr("Est_ProduccionAD")
        ObjModel.Est_HuevoAcAD = dr("Est_HuevoAcAD")
        ObjModel.Est_HuevoAcAA = dr("Est_HuevoAcAA")
        ObjModel.Est_MasaHuevoAcAA = dr("Est_MasaHuevoAcAA")
        ObjModel.Est_PesoHuevo = dr("Est_PesoHuevo")

        'Bitacora
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function


End Class
