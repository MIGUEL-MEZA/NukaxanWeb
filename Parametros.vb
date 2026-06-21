Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Parametros
    Public strError As String = ""
    Public Function GetSQL(CvePlataforma As Integer, CveParametro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CvePlataforma int =" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CveParametro int =" + CveParametro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Parametros @CvePlataforma,@CveParametro,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CvePlataforma As Integer, CveParametro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CvePlataforma, CveParametro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CvePlataforma As Integer, CveParametro As Integer) As List(Of ParametrosModel)
        Dim dt As DataTable
        Dim lst As New List(Of ParametrosModel)
        Try
            dt = FindAll(CvePlataforma, CveParametro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As ParametrosModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CvePlataforma As Integer, CveParametro As Integer) As ParametrosModel
        Dim ObjM As New ParametrosModel
        Dim dt As DataTable
        Try
            dt = FindAll(CvePlataforma, CveParametro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As ParametrosModel
        Dim ObjModel As New ParametrosModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.NomParametro = dr("NomParametro")
        ObjModel.Valor = dr("Valor")

        'Bitacora
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")
        Return ObjModel
    End Function
End Class
