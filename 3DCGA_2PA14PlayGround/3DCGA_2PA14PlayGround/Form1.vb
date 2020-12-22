Public Structure TPoint
    Dim x, y, z, w As Double
End Structure

Public Structure TLine
    Dim p1, p2 As Integer
End Structure

Public Structure TSurface
    Dim v1, v2, v3 As Integer
    Dim surf_color As Color
End Structure

Public Structure Tvector
    Dim dir_x, dir_y, dir_z As Double
End Structure

Public Structure Tslot2
    Public ymin, ymax As Integer
    Public dx, dy, xofymin As Double
End Structure

Public Class Form1
    Dim btmap As New Drawing.Bitmap(800, 800)
    Dim grph As Graphics = Graphics.FromImage(btmap)
    Dim p As Pen = New Pen(Color.Black)

    Dim poly_tri As Cpolygon = New Cpolygon()
    Dim poly_square As Cpolygon = New Cpolygon()

    Dim dir_projection As Tvector

    Dim cent_x As Double = 0
    Dim cent_y As Double = 0
    Dim cent_z As Double = 0

    Public wt()() As Double = New Double(3)() {}
    Public vt()() As Double = New Double(3)() {}
    Public st()() As Double = New Double(3)() {}

    Public matrix_rot_z()() As Double = New Double(3)() {}
    Public matrix_rot_y()() As Double = New Double(3)() {}
    Public matrix_rot_x()() As Double = New Double(3)() {}

    Dim index As Integer
    Dim action, current_object As String
    Dim placeholder As String

    Dim val_x As Double = 0.15
    Dim val_y As Double = 0.15
    Dim val_z As Double = 0.15

    Dim trans_x As Double = 0
    Dim trans_y As Double = 0
    Dim trans_z As Double = 0

    Dim ymin_set, ymax_set As Integer

    Dim poly_SET As List(Of List(Of Tslot2)) = New List(Of List(Of Tslot2))

    Dim global_SET As List(Of List(Of Tslot2)) = New List(Of List(Of Tslot2))


    '=========== PROCEDURES ===========

    Public Sub set_matrix_row(ByRef matrix()() As Double, row_num As Integer, value0 As Double, value1 As Double, value2 As Double, value3 As Double)
        matrix(row_num) = New Double() {value0, value1, value2, value3}
    End Sub

    Public Sub set_vertex(ByRef list_vertex As List(Of TPoint), point_x As Double, point_y As Double, point_z As Double, Optional ByVal point_w As Double = 1)
        Dim dummy_point As TPoint
        dummy_point.x = point_x
        dummy_point.y = point_y
        dummy_point.z = point_z
        dummy_point.w = point_w

        list_vertex.Add(dummy_point)
    End Sub

    Public Sub set_edge(ByRef list_edge As List(Of TLine), starting_point As Integer, ending_point As Integer)
        Dim dummy_line As TLine
        dummy_line.p1 = starting_point
        dummy_line.p2 = ending_point

        list_edge.Add(dummy_line)
    End Sub

    Public Sub set_surface(ByRef list_surface As List(Of TSurface), vertex1 As Integer, vertex2 As Integer, vertex3 As Integer, s_color As Color)
        Dim dummy_surface As TSurface
        dummy_surface.v1 = vertex1
        dummy_surface.v2 = vertex2
        dummy_surface.v3 = vertex3
        dummy_surface.surf_color = s_color

        list_surface.Add(dummy_surface)
    End Sub

    Public Sub set_vector(ByRef some_vector As Tvector, val_dir_x As Double, val_dir_y As Double, val_dir_z As Double)
        some_vector.dir_x = val_dir_x
        some_vector.dir_y = val_dir_y
        some_vector.dir_z = val_dir_z
    End Sub

    Public Sub clear_screen()
        pbox_screen.Image = Nothing
        grph.Clear(Color.White)
    End Sub

    Public Sub all_button_default_color()
        btn_up.BackColor = Color.White
        btn_up.ForeColor = Color.Black

        btn_down.BackColor = Color.White
        btn_down.ForeColor = Color.Black

        btn_right.BackColor = Color.White
        btn_right.ForeColor = Color.Black

        btn_left.BackColor = Color.White
        btn_left.ForeColor = Color.Black

        btn_in.BackColor = Color.White
        btn_in.ForeColor = Color.Black

        btn_out.BackColor = Color.White
        btn_out.ForeColor = Color.Black
    End Sub

    Public Sub default_cent()
        cent_x = 0
        cent_y = 0
        cent_z = 0
    End Sub

    Public Sub find_cent(ByVal list_vert As List(Of TPoint))
        For Each item As TPoint In list_vert
            cent_x += item.x
            cent_y += item.y
            cent_z += item.z
        Next

        cent_x /= list_vert.Count
        cent_y /= list_vert.Count
        cent_z /= list_vert.Count
    End Sub

    Public Sub transform_vertices_tri()
        poly_tri.clear_list()

        lbox_vertri_scs.Items.Clear()
        lbox_vertri_vcs.Items.Clear()

        For index = 0 To poly_tri.vertices.Count - 1
            poly_tri.VW.Add(matrix_mult_1x4(poly_tri.vertices.Item(index), wt))
            poly_tri.VR.Add(matrix_mult_1x4(poly_tri.VW.Item(index), vt))
            poly_tri.VS.Add(matrix_mult_1x4_VS(poly_tri.VR.Item(index), st))

            lbox_vertri_scs.Items.Add(CStr(index) + " ==> (" + CStr(CInt(poly_tri.VS.Item(index).x)) + " , " + CStr(CInt(poly_tri.VS.Item(index).y)) + " , " + CStr(CInt(poly_tri.VS.Item(index).z)) + ")")
            lbox_vertri_vcs.Items.Add(CStr(index) + " ==> (" + CStr(poly_tri.VR.Item(index).x) + " , " + CStr(poly_tri.VR.Item(index).y) + " , " + CStr(poly_tri.VR.Item(index).z) + ")")

            poly_tri.vertices.Item(index) = poly_tri.VW.Item(index)
        Next

        'Process the surfaces with backface culling
        For index = 0 To poly_tri.surfaces.Count - 1
            If back_face_culling(poly_tri.surfaces.Item(index), poly_tri.VR) Then
                poly_tri.after_bf.Add(poly_tri.surfaces.Item(index))
            End If
        Next
    End Sub

    Public Sub transform_vertices_square()
        poly_square.clear_list()

        lbox_versquare_scs.Items.Clear()
        lbox_versquare_vcs.Items.Clear()

        For index = 0 To poly_square.vertices.Count - 1
            poly_square.VW.Add(matrix_mult_1x4(poly_square.vertices.Item(index), wt))
            poly_square.VR.Add(matrix_mult_1x4(poly_square.VW.Item(index), vt))
            poly_square.VS.Add(matrix_mult_1x4_VS(poly_square.VR.Item(index), st))

            lbox_versquare_scs.Items.Add(CStr(index) + " ==> (" + CStr(CInt(poly_square.VS.Item(index).x)) + " , " + CStr(CInt(poly_square.VS.Item(index).y)) + " , " + CStr(CInt(poly_square.VS.Item(index).z)) + ")")
            lbox_versquare_vcs.Items.Add(CStr(index) + " ==> (" + CStr(poly_square.VR.Item(index).x) + " , " + CStr(poly_square.VR.Item(index).y) + " , " + CStr(poly_square.VR.Item(index).z) + ")")

            poly_square.vertices.Item(index) = poly_square.VW.Item(index)
        Next

        'Process the surface with backface culling
        For index = 0 To poly_square.surfaces.Count - 1
            If back_face_culling(poly_square.surfaces.Item(index), poly_square.VR) Then
                poly_square.after_bf.Add(poly_square.surfaces.Item(index))
            End If
        Next
    End Sub

    Public Sub set_SET_components(ByRef some_slot As Tslot2, ByVal point1 As TPoint, ByVal point2 As TPoint)
        If point1.y < point2.y Then
            some_slot.ymin = point1.y

            some_slot.xofymin = CDbl(point1.x)

            some_slot.ymax = point2.y

            some_slot.dx = CDbl(point2.x - point1.x)
            some_slot.dy = CDbl(point2.y - point1.y)

        ElseIf point1.y > point2.y Then
            some_slot.ymin = point2.y

            some_slot.xofymin = CDbl(point2.x)

            some_slot.ymax = point1.y

            some_slot.dx = CDbl(point1.x - point2.x)
            some_slot.dy = CDbl(point1.y - point2.y)

        Else
            some_slot.ymin = -1
        End If
    End Sub



    Public Sub generate_SET(ByVal current_surface As TSurface, ByVal type_polygon As String)
        Dim p(2) As TPoint
        Dim index As Integer

        poly_SET = New List(Of List(Of Tslot2))

        If type_polygon = "poly_tri" Then
            p(0) = poly_tri.VS.Item(current_surface.v1)
            p(1) = poly_tri.VS.Item(current_surface.v2)
            p(2) = poly_tri.VS.Item(current_surface.v3)

        ElseIf type_polygon = "poly_square" Then
            p(0) = poly_square.VS.Item(current_surface.v1)
            p(1) = poly_square.VS.Item(current_surface.v2)
            p(2) = poly_square.VS.Item(current_surface.v3)
        End If

        'slot1 is for line p0p1
        'slot2 is for line p1p2
        'slot3 is for line p2p3
        Dim slot1, slot2, slot3 As New Tslot2

        ymin_set = p(0).y
        ymax_set = p(0).y

        'Determine the lowest bound of the SET and the highest bound of the SET (the y)
        For index = 1 To 2
            If p(index).y < ymin_set Then
                ymin_set = p(index).y
            ElseIf p(index).y > ymax_set Then
                ymax_set = p(index).y
            End If
        Next

        set_SET_components(slot1, p(0), p(1))
        set_SET_components(slot2, p(1), p(2))
        set_SET_components(slot3, p(2), p(0))

        Dim list_slot As List(Of Tslot2) = New List(Of Tslot2)

        For index = ymin_set To ymax_set
            If slot1.ymin = index Then
                list_slot.Add(slot1)
            End If
            If slot2.ymin = index Then
                list_slot.Add(slot2)
            End If
            If slot3.ymin = index Then
                list_slot.Add(slot3)
            End If

            If list_slot.Count = 0 Then
                poly_SET.Add(New List(Of Tslot2))
            Else
                poly_SET.Add(list_slot)
            End If

            list_slot = New List(Of Tslot2)
        Next

        Dim x As Integer
        x = 10
    End Sub

    Public Sub initiate_AEL(ByVal draw_color As Color)
        Dim list_AEL As New List(Of Tslot2)
        Dim y As Integer

        pbox_screen.Image = btmap

        Dim index As Integer = 0
        Dim rem_checker As Integer = 0

        For y = ymin_set To ymax_set - 1
            'Check whether a slot is expired or not.
            'If it is expired, remove from AEL. 
            If list_AEL.Count <> 0 Then
                rem_checker = 0

                While rem_checker <= list_AEL.Count - 1
                    If list_AEL.Item(rem_checker).ymax = y Then
                        list_AEL.RemoveAt(rem_checker)
                    Else
                        rem_checker += 1
                    End If
                End While
            End If

            'Update the x of y min of every slot in AEL
            If list_AEL.Count <> 0 Then
                For Each item As Tslot2 In list_AEL
                    item.xofymin += item.dx / item.dy
                Next

                For index = 0 To list_AEL.Count - 1
                    Dim temp_slot As Tslot2
                    temp_slot = list_AEL.Item(index)
                    temp_slot.xofymin = list_AEL.Item(index).xofymin + (list_AEL.Item(index).dx / list_AEL.Item(index).dy)
                    list_AEL.Item(index) = temp_slot
                Next
            End If

            'If there is any slot in the current element of SET,--
            '-- add it to the AEL. 
            If poly_SET.Item(y - ymin_set).Count <> 0 Then
                For Each item As Tslot2 In poly_SET.Item(y - ymin_set)
                    list_AEL.Add(item)
                Next
            End If

            If list_AEL.Count <> 0 Then
                p = New Pen(draw_color)
                grph.DrawLine(p, CInt(list_AEL.Item(0).xofymin), y, CInt(list_AEL.Item(1).xofymin), y)
            End If
        Next
    End Sub

    Public Sub draw_poly()
        pbox_screen.Image = btmap
        For index = 0 To poly_tri.after_bf.Count - 1
            Dim p1, p2, p3 As TPoint
            p1 = poly_tri.VS.Item(poly_tri.after_bf.Item(index).v1)
            p2 = poly_tri.VS.Item(poly_tri.after_bf.Item(index).v2)
            p3 = poly_tri.VS.Item(poly_tri.after_bf.Item(index).v3)

            p = New Pen(poly_tri.after_bf.Item(index).surf_color)

            grph.DrawLine(p, CInt(p1.x), CInt(p1.y), CInt(p2.x), CInt(p2.y))
            grph.DrawLine(p, CInt(p2.x), CInt(p2.y), CInt(p3.x), CInt(p3.y))
            grph.DrawLine(p, CInt(p3.x), CInt(p3.y), CInt(p1.x), CInt(p1.y))
        Next

        For index = 0 To poly_square.after_bf.Count - 1
            Dim p1, p2, p3 As TPoint
            p1 = poly_square.VS.Item(poly_square.after_bf.Item(index).v1)
            p2 = poly_square.VS.Item(poly_square.after_bf.Item(index).v2)
            p3 = poly_square.VS.Item(poly_square.after_bf.Item(index).v3)

            p = New Pen(poly_square.after_bf.Item(index).surf_color)

            grph.DrawLine(p, CInt(p1.x), CInt(p1.y), CInt(p2.x), CInt(p2.y))
            grph.DrawLine(p, CInt(p2.x), CInt(p2.y), CInt(p3.x), CInt(p3.y))
            grph.DrawLine(p, CInt(p3.x), CInt(p3.y), CInt(p1.x), CInt(p1.y))
        Next


        ''Fill the polygon here

        ''Fill the triangular pyramid
        'For Each surface As TSurface In poly_tri.after_bf
        '    'Process SET
        '    generate_SET(surface, "poly_tri")

        '    'Process AEL
        '    initiate_AEL(surface.surf_color)
        'Next

        ''Fill the square pyramid
        'For Each surface As TSurface In poly_square.after_bf
        '    'Process SET
        '    generate_SET(surface, "poly_square")

        '    'Process AEL
        '    initiate_AEL(surface.surf_color)
        'Next


        'Use the scanline method from here. 
        'Procedure to insert all visible edges to the global SET

        global_SET = New List(Of List(Of Tslot2))

        insert_to_globalSET()

    End Sub

    Public Sub insert_to_globalSET()
        Dim p(2) As TPoint
        Dim list_slot As List(Of Tslot2) = New List(Of Tslot2)
        Dim index As Integer
        Dim first_iterate As Boolean = True


        For Each surface As TSurface In poly_tri.after_bf
            p(0) = poly_tri.VS.Item(surface.v1)
            p(1) = poly_tri.VS.Item(surface.v2)
            p(2) = poly_tri.VS.Item(surface.v3)

            'The first_iterate is used to set the first value for ymin_set and y_max set
            'This assignment act as a comparison in the future fot the other y's
            If first_iterate = True Then
                ymin_set = p(0).y
                ymax_set = p(0).y
                first_iterate = False
            Else
                'Check point per point on the current surface whether--
                '-- the previous point's y is larger or smaller than the current point's y
                'Assign the ymin and ymax accordingly
                If p(0).y < ymin_set Then
                    ymin_set = p(0).y
                ElseIf p(1).y < ymin_set Then
                    ymin_set = p(1).y
                ElseIf p(2).y < ymin_set Then
                    ymin_set = p(2).y
                End If

                If p(0).y > ymax_set Then
                    ymax_set = p(0).y
                ElseIf p(1).y > ymax_set Then
                    ymax_set = p(1).y
                ElseIf p(2).y > ymax_set Then
                    ymax_set = p(2).y
                End If
            End If

            list_slot.Add(set_SET_components_forGlobalSET(p(0), p(1)))
            list_slot.Add(set_SET_components_forGlobalSET(p(1), p(2)))
            list_slot.Add(set_SET_components_forGlobalSET(p(2), p(0)))
        Next

        For Each surface As TSurface In poly_square.after_bf
            p(0) = poly_square.VS.Item(surface.v1)
            p(1) = poly_square.VS.Item(surface.v2)
            p(2) = poly_square.VS.Item(surface.v3)

            If p(0).y < ymin_set Then
                ymin_set = p(0).y
            ElseIf p(1).y < ymin_set Then
                ymin_set = p(1).y
            ElseIf p(2).y < ymin_set Then
                ymin_set = p(2).y
            End If

            If p(0).y > ymax_set Then
                ymax_set = p(0).y
            ElseIf p(1).y > ymax_set Then
                ymax_set = p(1).y
            ElseIf p(2).y > ymax_set Then
                ymax_set = p(2).y
            End If

            list_slot.Add(set_SET_components_forGlobalSET(p(0), p(1)))
            list_slot.Add(set_SET_components_forGlobalSET(p(1), p(2)))
            list_slot.Add(set_SET_components_forGlobalSET(p(2), p(0)))
        Next

        Dim dummy_slot As List(Of Tslot2) = New List(Of Tslot2)

        'pass
        For index = ymin_set To ymax_set
            Dim x As Integer
            x = 10
        Next

    End Sub

    '=========== END PROCEDURES ===========


    '=========== FUNCTIONS ===========

    Public Function matrix_mult_4x4(matrix_a()() As Double, matrix_b()() As Double)
        Dim result_matrix(3)() As Double
        Dim temp_matrix(3) As Double
        Dim current As Double = 0

        Dim row, col, item As Integer

        For row = 0 To 3
            For col = 0 To 3
                For item = 0 To 3
                    current += matrix_a(row)(item) * matrix_b(item)(col)
                Next
                temp_matrix(col) = current
                current = 0
            Next
            result_matrix(row) = temp_matrix
            ReDim temp_matrix(3)
        Next

        Return result_matrix

    End Function

    Public Function matrix_mult_1x4(point_a As TPoint, matrix_b()() As Double)
        Dim dummy_matrix(3) As Double
        dummy_matrix(0) = point_a.x
        dummy_matrix(1) = point_a.y
        dummy_matrix(2) = point_a.z
        dummy_matrix(3) = point_a.w

        Dim result_matrix(3) As Double
        Dim point_b, point_c As TPoint
        Dim current As Double
        Dim index1, index2 As Integer

        current = 0

        For index1 = 0 To 3
            For index2 = 0 To 3
                current += dummy_matrix(index2) * matrix_b(index2)(index1)
            Next
            result_matrix(index1) = current
            current = 0
        Next

        point_b.x = result_matrix(0)
        point_b.y = result_matrix(1)
        point_b.z = result_matrix(2)
        point_b.w = result_matrix(3)

        If point_b.w <> 1 Then
            point_c.w = point_b.w / point_b.w
            point_c.x = point_b.x / point_b.w
            point_c.y = point_b.y / point_b.w
            point_c.z = point_b.z / point_b.w

            Return point_c
        End If

        Return point_b

    End Function

    Public Function matrix_mult_1x4_VS(point_a As TPoint, matrix_b()() As Double)
        Dim dummy_matrix(3) As Double
        dummy_matrix(0) = point_a.x
        dummy_matrix(1) = point_a.y
        dummy_matrix(2) = point_a.z
        dummy_matrix(3) = point_a.w

        Dim result_matrix(3) As Double
        Dim point_b, point_c As TPoint
        Dim current As Double
        Dim index1, index2 As Integer

        current = 0

        For index1 = 0 To 3
            For index2 = 0 To 3
                current += dummy_matrix(index2) * matrix_b(index2)(index1)
            Next
            result_matrix(index1) = Math.Round(current)
            current = 0
        Next

        point_b.x = result_matrix(0)
        point_b.y = result_matrix(1)
        point_b.z = result_matrix(2)
        point_b.w = result_matrix(3)

        If point_b.w <> 1 Then
            point_c.w = Math.Round(point_b.w / point_b.w)
            point_c.x = Math.Round(point_b.x / point_b.w)
            point_c.y = Math.Round(point_b.y / point_b.w)
            point_c.z = Math.Round(point_b.z / point_b.w)

            Return point_c
        End If

        Return point_b

    End Function

    Public Function deg_to_rad(ByVal deg As Double)
        Return deg * (Math.PI / 180)
    End Function

    Public Function find_vector(ByVal point1 As TPoint, ByVal point2 As TPoint)
        Dim result_vector As Tvector
        result_vector.dir_x = point2.x - point1.x
        result_vector.dir_y = point2.y - point1.y
        result_vector.dir_z = point2.z - point1.z

        Return result_vector
    End Function

    Public Function find_dot_product(ByVal vector1 As Tvector, ByVal vector2 As Tvector)
        Dim result As Double
        result = (vector1.dir_x * vector2.dir_x) + (vector1.dir_y * vector2.dir_y) + (vector1.dir_z * vector2.dir_z)

        Return result
    End Function

    Public Function find_cross_product(ByVal vector1 As Tvector, ByVal vector2 As Tvector)
        Dim result_vector As Tvector
        result_vector.dir_x = (vector1.dir_y * vector2.dir_z) - (vector2.dir_y * vector1.dir_z)
        result_vector.dir_y = (vector1.dir_z * vector2.dir_x) - (vector2.dir_z * vector1.dir_x)
        result_vector.dir_z = (vector1.dir_x * vector2.dir_y) - (vector2.dir_x * vector1.dir_y)

        Return result_vector
    End Function

    Public Function back_face_culling(ByVal surface As TSurface, vr_vertices As List(Of TPoint))
        Dim normal, vector1, vector2 As Tvector
        Dim result As Double

        vector1 = find_vector(vr_vertices.Item(surface.v1), vr_vertices.Item(surface.v2))
        vector2 = find_vector(vr_vertices.Item(surface.v1), vr_vertices.Item(surface.v3))

        normal = find_cross_product(vector1, vector2)

        result = find_dot_product(dir_projection, normal)

        If result < 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function set_SET_components_forGlobalSET(ByVal point1 As TPoint, ByVal point2 As TPoint)
        Dim some_slot As Tslot2
        If point1.y < point2.y Then
            some_slot.ymin = point1.y

            some_slot.xofymin = CDbl(point1.x)

            some_slot.ymax = point2.y

            some_slot.dx = CDbl(point2.x - point1.x)
            some_slot.dy = CDbl(point2.y - point1.y)

        ElseIf point1.y > point2.y Then
            some_slot.ymin = point2.y

            some_slot.xofymin = CDbl(point2.x)

            some_slot.ymax = point1.y

            some_slot.dx = CDbl(point1.x - point2.x)
            some_slot.dy = CDbl(point1.y - point2.y)

        Else
            some_slot.ymin = -1
        End If

        Return some_slot
    End Function


    '=========== END FUNCTIONS ===========

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pbox_screen.Image = btmap

        radbut_translate.Checked = True
        radbut_tri.Checked = True

        set_vector(dir_projection, 0.0, 0.0, -1.0)

        set_vertex(poly_tri.vertices, -3, 2, 0) '0
        set_vertex(poly_tri.vertices, -4, 0, 1) '1
        set_vertex(poly_tri.vertices, -2, 0, 1) '2
        set_vertex(poly_tri.vertices, -3, 0, -1) '3

        set_edge(poly_tri.edges, 0, 1)
        set_edge(poly_tri.edges, 1, 2)
        set_edge(poly_tri.edges, 2, 0)
        set_edge(poly_tri.edges, 2, 3)
        set_edge(poly_tri.edges, 3, 0)
        set_edge(poly_tri.edges, 3, 1)

        set_surface(poly_tri.surfaces, 0, 1, 2, Color.Red)
        set_surface(poly_tri.surfaces, 0, 2, 3, Color.Blue)
        set_surface(poly_tri.surfaces, 0, 3, 1, Color.Green)
        set_surface(poly_tri.surfaces, 1, 3, 2, Color.Yellow)

        set_vertex(poly_square.vertices, 3, 2, 0)
        set_vertex(poly_square.vertices, 2, 0, 1)
        set_vertex(poly_square.vertices, 4, 0, 1)
        set_vertex(poly_square.vertices, 4, 0, -1)
        set_vertex(poly_square.vertices, 2, 0, -1)

        set_edge(poly_square.edges, 0, 1)
        set_edge(poly_square.edges, 0, 2)
        set_edge(poly_square.edges, 0, 3)
        set_edge(poly_square.edges, 0, 4)
        set_edge(poly_square.edges, 1, 2)
        set_edge(poly_square.edges, 2, 3)
        set_edge(poly_square.edges, 3, 4)
        set_edge(poly_square.edges, 4, 1)

        set_surface(poly_square.surfaces, 0, 1, 2, Color.Purple)
        set_surface(poly_square.surfaces, 0, 2, 3, Color.Cyan)
        set_surface(poly_square.surfaces, 0, 3, 4, Color.CornflowerBlue)
        set_surface(poly_square.surfaces, 0, 4, 1, Color.Chartreuse)
        set_surface(poly_square.surfaces, 1, 4, 2, Color.Orange)
        set_surface(poly_square.surfaces, 2, 4, 3, Color.Orange)

        set_matrix_row(wt, 0, 1, 0, 0, 0)
        set_matrix_row(wt, 1, 0, 1, 0, 0)
        set_matrix_row(wt, 2, 0, 0, 1, 0)
        set_matrix_row(wt, 3, 0, 0, 0, 1)

        set_matrix_row(vt, 0, 1, 0, 0, 0)
        set_matrix_row(vt, 1, 0, 1, 0, 0)
        set_matrix_row(vt, 2, 0, 0, 1, -0.2)
        set_matrix_row(vt, 3, 0, 0, 0, 1)

        set_matrix_row(st, 0, 25, 0, 0, 0)
        set_matrix_row(st, 1, 0, -25, 0, 0)
        set_matrix_row(st, 2, 0, 0, 25, 0)
        set_matrix_row(st, 3, 200, 200, 0, 1)

        transform_vertices_tri()

        transform_vertices_square()

        draw_poly()

        Dim sample_list As List(Of String) = New List(Of String)
        sample_list.Add("Alvin")
        sample_list.Add("Jones")
        sample_list.Add("Cefin")

        If sample_list.Contains("Alvina") Then
            tes_label1.Text = "Truuueeee"
        Else
            tes_label1.Text = "Fallseeee"
        End If
    End Sub

    Private Sub Radbut_translate_CheckedChanged(sender As Object, e As EventArgs) Handles radbut_translate.CheckedChanged
        action = "translate"
    End Sub

    Private Sub Radbut_rotate_CheckedChanged(sender As Object, e As EventArgs) Handles radbut_rotate.CheckedChanged
        action = "rotate"
    End Sub

    Private Sub Radbut_tri_CheckedChanged(sender As Object, e As EventArgs) Handles radbut_tri.CheckedChanged
        current_object = "triangle"
    End Sub

    Private Sub Radbut_square_CheckedChanged(sender As Object, e As EventArgs) Handles radbut_square.CheckedChanged
        current_object = "square"
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If current_object = "triangle" Then
            If action = "translate" Then
                Select Case ChrW(e.KeyCode)
                    Case "W"
                        'Up
                        clear_screen()
                        btn_up.BackColor = Color.Red
                        btn_up.ForeColor = Color.White

                        trans_y += val_y

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, val_y, 0, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "S"
                        'Down
                        clear_screen()
                        btn_down.BackColor = Color.Red
                        btn_down.ForeColor = Color.White

                        trans_y -= val_y

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, -val_y, 0, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "A"
                        'Left
                        clear_screen()
                        btn_left.BackColor = Color.Red
                        btn_left.ForeColor = Color.White

                        trans_x -= val_x

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -val_x, 0, 0, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "D"
                        'Right
                        clear_screen()
                        btn_right.BackColor = Color.Red
                        btn_right.ForeColor = Color.White

                        trans_x += val_x

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, val_x, 0, 0, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "Q"
                        'Out
                        clear_screen()
                        btn_out.BackColor = Color.Red
                        btn_out.ForeColor = Color.White

                        trans_z += val_z

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, val_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "E"
                        'In
                        clear_screen()
                        btn_in.BackColor = Color.Red
                        btn_in.ForeColor = Color.White

                        trans_z -= val_z

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, -val_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                End Select
            ElseIf action = "rotate" Then
                Select Case ChrW(e.KeyCode)
                    Case "W"
                        'Up
                        clear_screen()
                        btn_up.BackColor = Color.Red
                        btn_up.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, Math.Cos(deg_to_rad(-1)), Math.Sin(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 2, 0, -Math.Sin(deg_to_rad(-1)), Math.Cos(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "S"
                        'Down
                        clear_screen()
                        btn_down.BackColor = Color.Red
                        btn_down.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, Math.Cos(deg_to_rad(1)), Math.Sin(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 2, 0, -Math.Sin(deg_to_rad(1)), Math.Cos(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "A"
                        'Left
                        clear_screen()
                        btn_left.BackColor = Color.Red
                        btn_left.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(1)), 0, -Math.Sin(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, Math.Sin(deg_to_rad(1)), 0, Math.Cos(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "D"
                        'Right
                        clear_screen()
                        btn_right.BackColor = Color.Red
                        btn_right.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(-1)), 0, -Math.Sin(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, Math.Sin(deg_to_rad(-1)), 0, Math.Cos(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "Q"
                        'Rotate counter-clockwise
                        clear_screen()
                        btn_out.BackColor = Color.Red
                        btn_out.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(1)), Math.Sin(deg_to_rad(1)), 0, 0)
                        set_matrix_row(wt, 1, -Math.Sin(deg_to_rad(1)), Math.Cos(deg_to_rad(1)), 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                    Case "E"
                        'Rotate clockwise
                        clear_screen()
                        btn_in.BackColor = Color.Red
                        btn_in.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_tri.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(-1)), Math.Sin(deg_to_rad(-1)), 0, 0)
                        set_matrix_row(wt, 1, -Math.Sin(deg_to_rad(-1)), Math.Cos(deg_to_rad(-1)), 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_tri()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_tri()

                        draw_poly()
                End Select
            End If

        ElseIf current_object = "square" Then
            If action = "translate" Then
                Select Case ChrW(e.KeyCode)
                    Case "W"
                        'Up
                        clear_screen()
                        btn_up.BackColor = Color.Red
                        btn_up.ForeColor = Color.White

                        trans_y += val_y

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, val_y, 0, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "S"
                        'Down
                        clear_screen()
                        btn_down.BackColor = Color.Red
                        btn_down.ForeColor = Color.White

                        trans_y -= val_y

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, -val_y, 0, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "A"
                        'Left
                        clear_screen()
                        btn_left.BackColor = Color.Red
                        btn_left.ForeColor = Color.White

                        trans_x -= val_x

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -val_x, 0, 0, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "D"
                        'Right
                        clear_screen()
                        btn_right.BackColor = Color.Red
                        btn_right.ForeColor = Color.White

                        trans_x += val_x

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, val_x, 0, 0, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "Q"
                        'Out
                        clear_screen()
                        btn_out.BackColor = Color.Red
                        btn_out.ForeColor = Color.White

                        trans_z += val_z

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, val_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "E"
                        'In
                        clear_screen()
                        btn_in.BackColor = Color.Red
                        btn_in.ForeColor = Color.White

                        trans_z -= val_z

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, -val_z, 1)

                        transform_vertices_square()

                        draw_poly()
                End Select
            ElseIf action = "rotate" Then
                Select Case ChrW(e.KeyCode)
                    Case "W"
                        'Up
                        clear_screen()
                        btn_up.BackColor = Color.Red
                        btn_up.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, Math.Cos(deg_to_rad(-1)), Math.Sin(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 2, 0, -Math.Sin(deg_to_rad(-1)), Math.Cos(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "S"
                        'Down
                        clear_screen()
                        btn_down.BackColor = Color.Red
                        btn_down.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, Math.Cos(deg_to_rad(1)), Math.Sin(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 2, 0, -Math.Sin(deg_to_rad(1)), Math.Cos(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "A"
                        'Left
                        clear_screen()
                        btn_left.BackColor = Color.Red
                        btn_left.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(1)), 0, -Math.Sin(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, Math.Sin(deg_to_rad(1)), 0, Math.Cos(deg_to_rad(1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "D"
                        'Right
                        clear_screen()
                        btn_right.BackColor = Color.Red
                        btn_right.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(-1)), 0, -Math.Sin(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, Math.Sin(deg_to_rad(-1)), 0, Math.Cos(deg_to_rad(-1)), 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "Q"
                        'Rotate counter-clockwise
                        clear_screen()
                        btn_out.BackColor = Color.Red
                        btn_out.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(1)), Math.Sin(deg_to_rad(1)), 0, 0)
                        set_matrix_row(wt, 1, -Math.Sin(deg_to_rad(1)), Math.Cos(deg_to_rad(1)), 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                    Case "E"
                        'Rotate clockwise
                        clear_screen()
                        btn_in.BackColor = Color.Red
                        btn_in.ForeColor = Color.White

                        default_cent()
                        find_cent(poly_square.vertices)

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, -cent_x, -cent_y, -cent_z, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, Math.Cos(deg_to_rad(-1)), Math.Sin(deg_to_rad(-1)), 0, 0)
                        set_matrix_row(wt, 1, -Math.Sin(deg_to_rad(-1)), Math.Cos(deg_to_rad(-1)), 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, 0, 0, 0, 1)

                        transform_vertices_square()

                        set_matrix_row(wt, 0, 1, 0, 0, 0)
                        set_matrix_row(wt, 1, 0, 1, 0, 0)
                        set_matrix_row(wt, 2, 0, 0, 1, 0)
                        set_matrix_row(wt, 3, cent_x, cent_y, cent_z, 1)

                        transform_vertices_square()

                        draw_poly()
                End Select
            End If
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        all_button_default_color()
    End Sub
End Class

Class Cpolygon
    Public vertices, VW, VR, VS As New List(Of TPoint)
    Public edges As New List(Of TLine)
    Public surfaces As New List(Of TSurface)

    Public after_bf As New List(Of TSurface)

    Public Sub clear_list()
        VW.Clear()
        VR.Clear()
        VS.Clear()
        after_bf.Clear()
    End Sub
End Class
