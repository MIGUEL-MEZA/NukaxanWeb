Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.DataBase
Imports NukaxanWEB.Libreria
Public Class Nireo_CatEquiposBIAS
    Public strError As String = ""
    Public Function GetSQL(CveEquipo As Integer, CveCategoria As Integer, CveParametro As Integer) As String
        Dim sb As New StringBuilder

        sb.Append(" DECLARE @CveEquipo  int=" + CveEquipo.ToString)
        sb.Append(" DECLARE @CveCategoriaP  int=" + CveCategoria.ToString)
        sb.Append(" DECLARE @CveParametro  int=" + CveParametro.ToString)
        sb.Append(" DECLARE @Estatus int=0")
        sb.Append(" DECLARE @Mensaje varchar(250)=''")

        sb.Append(" EXEC spc_Nireo_CatEquipos_BIAS @CveEquipo,@CveCategoriaP,@CveParametro,@Estatus Output,@Mensaje Output")

        Return sb.ToString
    End Function
    Public Function FindAll(CveEquipo As Integer, CveCategoria As Integer, CveParametro As Integer) As DataTable
        Dim dt As DataTable
        Try
            dt = execQuery(GetSQL(CveEquipo, CveCategoria, CveParametro))
        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        End Try

        Return dt
    End Function
    Public Function FindlstAll(CveEquipo As Integer, CveCategoria As Integer, CveParametro As Integer) As List(Of Nireo_CatEquiposBIASModel)
        Dim dt As DataTable
        Dim lst As New List(Of Nireo_CatEquiposBIASModel)
        Try
            dt = FindAll(CveEquipo, CveCategoria, CveParametro)
            For Each dr As DataRow In dt.Rows
                Dim ObjM As Nireo_CatEquiposBIASModel = FillModel(dr)
                lst.Add(ObjM)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try

        Return lst
    End Function
    Public Function FindById(CveEquipo As Integer, CveCategoria As Integer, CveParametro As Integer) As Nireo_CatEquiposBIASModel
        Dim ObjM As New Nireo_CatEquiposBIASModel
        Dim dt As DataTable
        Try
            dt = FindAll(CveEquipo, CveCategoria, CveParametro)
            If Not dt Is Nothing Then ObjM = FillModel(dt.Rows(0))

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return ObjM
    End Function
    Public Function FillModel(dr As DataRow) As Nireo_CatEquiposBIASModel
        Dim ObjModel As New Nireo_CatEquiposBIASModel
        ObjModel.CveEquipo = dr("CveEquipo")
        ObjModel.CveCategoriaP = dr("CveCategoriaP")
        ObjModel.CveParametro = dr("CveParametro")
        ObjModel.BIAS = dr("BIAS")
        ObjModel.NomEquipo = dr("NomEquipo")
        ObjModel.Serie = dr("Serie")
        ObjModel.NomCategoriaP = dr("NomCategoriaP")
        ObjModel.NomParametro = dr("NomParametro")

        'Bitacora
        ObjModel.FecAct = CDate(dr("FecAct")).ToString("dd/MM/yyyy HH:mm")
        ObjModel.UsuAct = dr("UsuAct")
        ObjModel.NomUsuAct = dr("NomUsuAct")

        Return ObjModel
    End Function

    Public Function SaveModel(Valores As String, UsuAct As Int64) As Boolean
        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim IsResult As Boolean = False

        Try
            sb.Append(" DECLARE @Valores varchar(MAX)='" + Valores + "'")
            sb.Append(" DECLARE @UsuAct bigint=" + UsuAct.ToString)
            sb.Append(" DECLARE @Estatus int=0")
            sb.Append(" DECLARE @Mensaje varchar(250)=''")

            sb.Append(" EXEC spu_Nireo_CatEquipos_BIAS @Valores,@UsuAct,@Estatus Output,@Mensaje Output")

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
