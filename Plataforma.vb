Imports System.Data
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Plataforma
    Dim strSQLExe As String = ""
    Public strError As String = ""

    Public Function getSQL(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As String
        Dim sb As New StringBuilder
        sb.Append(" DECLARE @Rol int=" + Rol.ToString)
        sb.Append(" DECLARE @CvePlataforma int=" + CvePlataforma.ToString)
        sb.Append(" DECLARE @CvePlataformaP int=" + CvePlataformaP.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")
        sb.Append(" EXEC spc_Plataformas @Rol,@CvePlataforma,@CvePlataformaP,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As DataTable
        strError = ""
        Dim dt As DataTable
        Try
            strSQLExe = getSQL(Rol, CvePlataforma, CvePlataformaP)
            dt = execQuery(strSQLExe)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As List(Of PlataformaModel)
        Dim dt As DataTable
        Dim lst As New List(Of PlataformaModel)
        Try
            dt = FindAll(Rol, CvePlataforma, CvePlataformaP)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As PlataformaModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(Rol As Integer, CvePlataforma As Integer, CvePlataformaP As Integer) As PlataformaModel
        Dim dt As DataTable
        Dim ObjM As PlataformaModel
        Try
            dt = FindAll(Rol, CvePlataforma, CvePlataformaP)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As PlataformaModel
        Dim ObjModel As New PlataformaModel
        ObjModel.CvePlataforma = dr("CvePlataforma")
        ObjModel.CvePlataformaP = dr("CvePlataformaP")
        ObjModel.NomPlataforma = dr("NomPlataforma")
        ObjModel.Descripcion = dr("Descripcion")
        ObjModel.Posicion = dr("Posicion")
        ObjModel.Url = dr("Url")
        ObjModel.ImgPlataforma = dr("ImgPlataforma")
        ObjModel.CveEstatus = dr("CveEstatus")

        Return ObjModel
    End Function

End Class