Imports System.Data
Imports Microsoft.Ajax.Utilities
Imports NukaxanWEB
Imports NukaxanWEB.Libreria
Imports NukaxanWEB.DataBase
Imports System.Data.SqlTypes

Public Class Catalogos
    Dim strSQLExe As String = ""
    Public strError As String = ""
    'GENERALES
    Public Sub LlenaEstatusGeneral(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 1,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEstatus", "NomEstatus", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOpcionGeneral(DDLControl As Control, Categoria As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 2,' AND Categoria IN(''" + Categoria + "'')'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveOpcion", "NomOpcion", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPeriodo(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 3,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "Periodo", "NomPeriodo", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaMes(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 4,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveMes", "NomMes", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaClientesAll(DDLControl As Control, op As Integer, valorfiltro As String, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Dim filtro As String = ""
        Try
            If op = 1 Then filtro = " AND CodCliente NOT IN(SELECT CodCliente FROM PerfilCliente_Usuarios WHERE CodUsuario=''" + valorfiltro + "'' )"
            If op = 2 Then filtro = " AND CodCliente NOT IN(SELECT CodCliente FROM PerfilCliente_Plataformas WHERE CvePlataforma=" + valorfiltro + " )"
            filtro += " AND CveEstatus =" + CveEstatus.ToString
            sb.Append(" EXEC spc_Catalogos 17,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CodCliente", "NomCliente", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaMoneda(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 18,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveMoneda", "NomMoneda", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    Public Sub LlenaReportes_Modalidad(DDLControl As Control, CvePlataforma As Integer, CveReporte As Integer)
        Dim dt As New DataTable
        Dim sb As New StringBuilder
        Dim filtro As String = ""
        Try
            filtro = " AND CvePlataforma=" + CvePlataforma.ToString + " AND CveReporte=" + CveReporte.ToString
            sb.Append(" EXEC spc_Catalogos 25,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveModalidad", "NomModalidad", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaReportes_Indicadores(DDLControl As Control, CvePlataforma As Integer, CveReporte As Integer)
        Dim dt As New DataTable
        Dim sb As New StringBuilder
        Dim filtro As String = ""
        Try
            filtro = " AND CvePlataforma=" + CvePlataforma.ToString + " AND CveReporte=" + CveReporte.ToString
            sb.Append(" EXEC spc_Catalogos 26,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveIndicador", "NomIndicador", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'USUARIOS
    Public Sub LlenaRoles(DDLControl As Control, Optional filtro As String = "I", Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 5,' AND CveTipo=''" + filtro + "'' AND CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveRol", "NomRol", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPuestos(DDLControl As Control, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 6,' AND CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CvePuesto", "NomPuesto", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaUbicacion(DDLControl As Control, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 7,' AND CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveUbicacion", "NomUbicacion", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaArea(DDLControl As Control, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 8,' AND tbl.CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveArea", "NomArea", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaUsuarioInterno(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 9,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "userId", "NomUsuario", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaCliente_UsuarioInterno(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 15,'" + CodCliente + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CodUsuario", "NomUsuario", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaCliente_Equipos(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 16,'" + CodCliente + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEquipo", "NomEquipo", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'CLIENTES
    Public Sub LlenaGeneralClientes(DDLControl As Control, CvePlataforma As Integer, CodUsuario As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 19,'" + CvePlataforma.ToString + ",''" + CodUsuario + "'''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CodCliente", "NomCliente", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPlataforma(DDLControl As Control, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 10,' AND tbl.CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CvePlataforma", "NomPlataforma", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPlataforma2(DDLControl As Control, Optional CveEstatus As Integer = 1)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 10,' AND tbl.CveEstatus =" + CveEstatus.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "NomPlataforma", "NomPlataforma", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaCtePais(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 13,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "Pais", "NomPais", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaCteOrigen(DDLControl As Control, Optional filtro As String = "")
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 14,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveOrigen", "NomOrigen", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'PERFIL CLIENTE - GENERAL
    Public Sub LlenaClientes(DDLControl As Control, CodUsuario As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 11,' AND tbl.CodUsuario=''" + CodUsuario + "'''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CodCliente", "NomCliente", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPerfilCliente_CategoriaProductos(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 12,'" + CodCliente + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveCategoriaP", "NomCategoriaP", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaAjusteGDP(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 400,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "Rango", "NomRango", False)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'PERFIL CLIENTE - NIREO
    Public Sub LlenaPerfilCliente_Nireo_Productos(DDLControl As Control, CodCliente As String, CveCategoriaP As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 201,' AND CodCliente=''" + CodCliente + "''" + If(CveCategoriaP = "", "", " AND CveCategoriaP=" + CveCategoriaP) + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveProducto", "NomProducto", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPerfilCliente_Nireo_Productos2(DDLControl As Control, CodCliente As String, op As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 201,' AND CodCliente=''" + CodCliente + "'''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, If(op = 0, "CveProducto", "CodALLIX"), "NomProducto", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPerfilCliente_Nireo_Parametros(DDLControl As Control, CodCliente As String, CveProducto As String, op As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 202,' AND R.CodCliente=''" + CodCliente + "'' AND " + If(op = 0, "R.CveProducto=", "P.CodALLIX=''") + CveProducto + If(op = 0, "'", "'''"))
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, If(op = 0, "CveParametro", "CodALLIX"), "NomParametro", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaPerfilCliente_Nireo_Proveedores(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 203,' AND CodCliente=''" + CodCliente + "'''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveProveedor", "NomProveedor", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'NIREO
    Public Sub LlenaNireo_Origen(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 24,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveOrigen", "NomOrigen", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaNireo_CategoriaP(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 20,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveCategoriaP", "NomCategoriaP", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaNireo_Parametros(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 21,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveParametro", "NomParametro", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaNireo_ParametrosxCategoria(DDLControl As Control, CveCategoria As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 211,'" + CveCategoria + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveParametro", "NomParametro", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaProductos_Tipos(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 23,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveTipoP", "NomTipoP", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'OPTIMIZER-CERDOS
    Public Sub LlenaOptimizer_Modalidad(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 43,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveModalidad", "NomModalidad", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizer_Referencia(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 40,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveReferencia", "NomReferencia", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizer_Parametros(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 41,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveParametro", "NomParametro", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizer_EstadoSanitario(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 42,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEstado", "NomEstado", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'OPTIMIZER-POLLOS
    Public Sub LlenaOptimizerP_Referencia(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 402,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveReferencia", "NomReferencia", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizerP_Parametros(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 412,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveParametro", "NomParametro", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'OPTIMIZER-GALLINAS
    Public Sub LlenaOptimizerG_Referencia(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 430,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveReferencia", "NomReferencia", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizerG_EstatusConfort(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 432,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEstatusC", "NomEstatusC", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaOptimizerG_TipoInstalacion(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 433,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveTipoInstalacion", "NomTipoInstalacion", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    'PRODAVES
    Public Sub Llena_ProdAvesEstatus(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 547,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEstatus", "NomEstatus", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesEtapas(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 541,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveEtapa", "NomEtapa", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesFases(DDLControl As Control, CodCliente As String, CveEtapa As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            Dim filtro As String = " AND CodCliente = ''" + CodCliente + "'' AND CveEtapa=" + CveEtapa.ToString
            sb.Append(" EXEC spc_Catalogos 548,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveFase", "NomFase", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Function Obten_ProdAvesEtapas() As List(Of CatalogosModel)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Dim lst As New List(Of CatalogosModel)
        Try
            sb.Append(" EXEC spc_Catalogos 541,''")
            dt = execQuery(sb.ToString)
            For Each dr As DataRow In dt.Rows
                Dim ObjModel As New CatalogosModel
                ObjModel.Id = dr("CveEtapa")
                ObjModel.Nombre = dr("NomEtapa")
                lst.Add(ObjModel)
            Next

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
        Return lst
    End Function
    Public Sub Llena_ProdAvesLineaG(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 539,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveLineaG", "NomLineaG", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesGuia(DDLControl As Control)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 540,''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveGuia", "NomGuia", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesLoteOrigen(DDLControl As Control, CodCliente As String, CveLote As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            Dim filtro As String = " AND CodCliente = ''" + CodCliente + "''" + If(CveLote = 0, "", " AND CveLote<>" + CveLote.ToString)
            sb.Append(" EXEC spc_Catalogos 542,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveLote", "CodLote", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesLoteLineaG(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 543,'" + CodCliente + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveLineaG", "NomLineaG", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesLoteGuia(DDLControl As Control, CveLineaG As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 544,'" + CveLineaG.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveGuia", "NomGuia", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

    Public Sub Llena_ProdAvesLoteRecepcion(DDLControl As Control, CodCliente As String, CveLote As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            Dim filtro As String = " AND T1.CodCliente = ''" + CodCliente + "'' AND T1.CveLote=" + CveLote.ToString
            sb.Append(" EXEC spc_Catalogos 549,'" + filtro + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveLoteR", "NomCaseta", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub Llena_ProdAvesCiclo(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 550,'" + CodCliente + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveCiclo", "NomCiclo", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub


    'NULINK
    Public Sub LlenaProdAves_Granjas(DDLControl As Control, CodCliente As String)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 545,' AND CodCliente=''" + CodCliente + "'''")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveGranja", "NomGranja", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub
    Public Sub LlenaProdAves_Casetas(DDLControl As Control, CodCliente As String, CveGranja As Integer)
        Dim dt As DataTable
        Dim sb As New StringBuilder
        Try
            sb.Append(" EXEC spc_Catalogos 546,' AND CodCliente=''" + CodCliente + "''" + " AND CveGranja=" + CveGranja.ToString + "'")
            dt = execQuery(sb.ToString)
            Call subControl_fill(DDLControl, dt, "CveCaseta", "NomCaseta", True)

        Catch ex As Exception
            strError = CleanSpecialCharacter(ex.Message)
        Finally
            dt = Nothing
        End Try
    End Sub

End Class