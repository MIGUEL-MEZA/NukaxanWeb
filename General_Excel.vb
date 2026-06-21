Imports System.Data
Imports NukaxanWEB

Public Class General_Excel
    Public strError As String = ""
    Public Function GenerarArchivo(ObjE As General_ExcelModel) As StringBuilder
        Dim sb As New StringBuilder
        Try
            'Estilos------
            Dim estilo_titulo As String = "style='font-family:Arial;font-size: 32px;font-weight: normal;letter-spacing: 5px;color:#003F7C;'"
            Dim estilo_filtro_label As String = "style='font-family:Arial;font-size: 18px;font-weight: normal;letter-spacing: 0px;color:#44546A;'"
            Dim estilo_filtro_valor As String = "style='font-family:Arial;font-size: 18px;font-weight: normal;letter-spacing: 0px;color:#003F7C;'"
            Dim estilo_encabezado As String = "style='font-family:Arial;font-size: 13px;font-weight: normal;background-color: #003F7C;color: #FFFFFF;'"
            Dim estilo_valor As String = "style='font-family:Arial;font-size: 12px;font-weight: normal;background-color: #003F7C;color: #000000;'"

            sb.Append("<TABLE>")
            'Title-----------------
            sb.Append("<tr><td colspan='" + ObjE.lstColumnas.Count.ToString + "' align='left'>")
            sb.Append("<span " + estilo_titulo + "> " + ObjE.Titulo + "</span>")
            sb.Append("</td></tr>")
            'Filtros-----------------
            sb.Append("<tr><td colspan='" + ObjE.lstColumnas.Count.ToString + "' align='center'>")
            For i As Integer = 0 To ObjE.lstFiltroEtiquetas.Count - 1
                If i <> 0 Then sb.Append("&nbsp;&nbsp;&nbsp;")
                sb.Append("<span " + estilo_filtro_label + ">" + ObjE.lstFiltroEtiquetas.Item(i) + " </span>")
                sb.Append("<span " + estilo_filtro_valor + "> " + ObjE.lstFiltroValores.Item(i) + "</span>")
            Next
            sb.Append("</td></tr>")
            'Header-----------------
            sb.Append("<tr>")
            For i As Integer = 0 To ObjE.lstColumnas.Count - 1
                sb.Append("<td align='center' " + estilo_encabezado + ">" + ObjE.lstColumnas.Item(i).Split("|")(1) + "</td>")
            Next
            sb.Append("</tr>")

            'Detalle-----------------
            Dim rowAlternative As String = "style='background-color:#EAEAEA;'"
            If ObjE.RowEncabezado(0) <> "0" Then rowAlternative = "style='background-color:#EAEAEA;font-weight: bold;'"

            For i As Integer = 0 To ObjE.lstValores.Count - 1
                Dim rowheader As Boolean = If(ObjE.RowEncabezado.IndexOf((i + 1).ToString) >= 0, True, False)
                sb.Append("<tr>")
                For j As Integer = 0 To ObjE.lstValores.Item(i).Count - 1
                    Dim tmp() As String = ObjE.lstColumnas.Item(j).Split("|")
                    If ObjE.RowEncabezado(0) = "0" Then
                        sb.Append("<td  align='" + If(tmp(0).ToLower = "c", "center", "left") + "' valign='top'" + If(CLng(i) Mod 2 > 0, "", rowAlternative) + If(ObjE.ColEmpiezaCero.IndexOf(j.ToString) >= 0, "class='textmode'", "") + " > " + ObjE.lstValores.Item(i).Item(j) + "</td>")
                    Else
                        If rowheader Then
                            sb.Append("<td  align='" + If(tmp(0).ToLower = "c", "center", "left") + "' valign='top'" + rowAlternative + If(ObjE.ColEmpiezaCero.IndexOf(j.ToString) >= 0, "class='textmode'", "") + " > " + ObjE.lstValores.Item(i).Item(j) + "</td>")
                        Else
                            sb.Append("<td  align='" + If(tmp(0).ToLower = "c", "center", "left") + "' valign='top'" + If(ObjE.ColEmpiezaCero.IndexOf(j.ToString) >= 0, "class='textmode'", "") + " > " + ObjE.lstValores.Item(i).Item(j) + "</td>")
                        End If
                    End If
                Next
                sb.Append("</tr>")
            Next
            'Totales---------------------
            If ObjE.ColTotales(0) <> "0" Then
                sb.Append("<tr>")
                sb.Append("<td align='left' colspan='2' " + estilo_encabezado + ">TOTAL</td>")
                For i As Integer = 2 To ObjE.lstColumnas.Count - 1
                    Dim ok As Boolean = False
                    For j = 0 To ObjE.ColTotales.Count - 1
                        If i = ObjE.ColTotales.Item(j) Then
                            sb.Append("<td align='center' " + estilo_encabezado + ">" + ObjE.Totales.Item(j) + "</td>")
                            ok = True
                            Exit For
                        End If
                    Next
                    If ok = False Then sb.Append("<td align='center' " + estilo_encabezado + ">&nbsp;</td>")
                Next
                sb.Append("</tr>")
            End If
            sb.Append("</TABLE>")

        Catch ex As Exception

        End Try
        Return sb
    End Function
End Class
