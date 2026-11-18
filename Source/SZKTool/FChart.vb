'============================================
'File:   FChart.vb
'Name:   グラフ描画クラス
'Desc:   グラフの描画を行う。
'Author: Fumihiko Kondo
'Date:   2015/04/16
'============================================

'''<summary>
'''グラフ描画用クラス。
'''</summary>
Public Class FChart
	'Implements IDisposable

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   定数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '''<summary> 最大表示線数。 </summary>
    Friend Const MAX_LINE As Integer = 100
	'''<summary> 最大データ点数。 </summary>
    Friend Const MAX_DAT As Integer = 10000

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   構造体
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '''<summary>
    '''軸の設定状態を表すクラス。
    '''</summary>
	Public Class FAxis
		'''<summary> 軸の現在表示範囲の最小値。(数値) </summary>
		Public MinVal As Double
		'''<summary> 軸の現在表示範囲の最大値。(数値) </summary>
		Public MaxVal As Double
		'''<summary> 軸の取り得る表示範囲の最小値。 </summary>
		Public LowestVal As Double = Double.MinValue
		'''<summary> 軸の取り得る表示範囲の最大値。 </summary>
		Public HighestVal As Double = Double.MaxValue
		'''<summary> 拡大時に軸が取りえる表示範囲の最小値。 </summary>
		Public NarrowestRange As Double = 1.0R
		'''<summary> 目盛りの本数。(未実装) </summary>
		Public ScaleLine As Integer
		'''<summary> 値表示付き目盛りの本数。(未実装) </summary>
		Public Incremental As Integer
		'''<summary> 軸描画時に使用するペン。 </summary>
		Public Pen As Pen = New Pen(Color.Black, 1)
		'''<summary> 軸の単位。 </summary>
		Public Text As String
		'''<summary> 軸の種類。0:数値 1:日付(X軸のみ) </summary>
		Public Kind As Integer
		'''<summary> 軸を有効にする。Y2でのみ有効 </summary>
		Public Enabled As Boolean
	End Class

	'''<summary>
	'''グラフ表示データを表すクラス。
	'''</summary>
	Public Class FCData
		'''<summary> Xデータ値。 </summary>
		Public X As Double()
		'''<summary> Yデータ値。 </summary>
		Public Y As Double()
		'''<summary> Stringsデータ値。 </summary>
		Public S As String()
		'''<summary> 点表示時の強調表示フラグ。 </summary>
		Public Bld As Boolean()
		'''<summary> データ描画時に使用するペン。 </summary>
		Public Pen As Pen = New Pen(Color.Black, 1)
		'''<summary> 汎用変数。 </summary>
		Public TagNum As Integer
		'''<summary> グラフの種類。データであるか、補助線であるか等を指定 </summary>
		Public Kind As Integer
		'''<summary> グラフ描画時に参照するY軸番号(線番号)。0:Y1, 1:Y2 </summary>
		Public RefAxis As Integer
		'''<summary> 線の選択状態。 </summary>
		Public Selected As Boolean = False
		'''<summary> グラフの描画方法。0:線 1:プロット </summary>
		Public DrawType As Integer = 0
		'''<summary> データのキー。追加項目の検索に使用可</summary>
        Public Key As String = ""
        '''<summary> プロットサイズ </summary>
        Public PltWidth As Single = 7.0
	End Class

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   API宣言
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   パブリック変数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '''<summary> グリッドの表示許可 </summary>
	Public bDispGrids As Boolean = True
	'''<summary> グリッドの表示許可 </summary>
	Public bDispAxis As Boolean = True
	'''<summary> グラフ周辺文字列の表示許可 </summary>
	Public bDispStrs As Boolean = True
	'''<summary> 左上に表示する文字列 </summary>
	Public strLT As String = ""
	'''<summary> 右下に表示する文字列 </summary>
	Public strRB As String = ""


	'''<summary> グリッド描画用のペン </summary>
	Public GridPen As Pen = New Pen(Color.LightGray, 1)
	'''<summary> 背景描画用ブラシ </summary>
	Public BGBrush As Brush = Brushes.White

	'''<summary> 変更を反映するPictureBox。初期化時に指定 </summary>
	Public WithEvents picbox As PictureBox
	'''<summary> PictureBoxはフォーカスを持てないため、
	'''フォーカスを代わりに得るためのラベルを設定する。 </summary>
	Public FocusGetterLabel As Label

	'''<summary> X軸。 </summary>
	Public XAxis As FAxis = New FAxis
	'''<summary> Y1軸。 </summary>
	Public Y1Axis As FAxis = New FAxis
	'''<summary> Y2軸。 </summary>
	Public Y2Axis As FAxis = New FAxis

	Public AxisFont As Font = New Font("Meiryo UI", 14)

	'''<summary> グラフの荒さ。1で省略なし </summary>
	Public Rough As Integer = 1

	'''<summary> グラフに表示するデータ。 </summary>
	Public Data As FCData() = New FCData(MAX_LINE) {}

	'''<summary> グラフ画像のPictureBox上での左上位置 </summary>
	Public GraphPosMin As PointF
	'''<summary> グラフ画像のPictureBox上での右下位置 </summary>
	Public GraphPosMax As PointF

	'''<summary> ユーザが左ドラッグでグラフ操作を可能かを設定する。 </summary>
	Public bCanLeftDrag As Boolean = True
	'''<summary> ユーザが右ドラッグでグラフ操作を可能かを設定する。 </summary>
	Public bCanRightDrag As Boolean = True

    '''<summary> 軸タイトルのフォント </summary>
    Public fntTitle As New Font("Arial", 14)

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   プロパティ
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    Private CursorPosXValue As Double = 0.0R
	'''<summary> マウスカーソルが示すX軸の値を取得する。 </summary>
	Public ReadOnly Property CursorPosX() As Double
		Get
			Return CursorPosXValue
		End Get
	End Property

	Private CursorPosY1Value As Double = 0.0R
	'''<summary> マウスカーソルが示すY1軸の値を取得する。 </summary>
	Public ReadOnly Property CursorPosY1() As Double
		Get
			Return CursorPosY1Value
		End Get
	End Property

	Private CursorPosY2Value As Double = 0.0R
	'''<summary> マウスカーソルが示すY2軸の値を取得する。 </summary>
	Public ReadOnly Property CursorPosY2() As Double
		Get
			Return CursorPosY2Value
		End Get
	End Property

    '▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
    '   プライベート変数
    '▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

    '''<summary>
    '''グラフ画面表示用座標。Y軸の方向に注意
    '''0.0～1.0の範囲で座標を指定
    '''描画時にサイズに応じて拡大する
    '''(線番号)(要素番号)
    '''</summary>
    Private m_GraphDP()() As PointF = New PointF(MAX_LINE)() {}

    '''<summary> マウス左ボタンの状態。 </summary>
    Private m_bMouseLState As Boolean = False
    '''<summary> マウス中ボタンの状態。 </summary>
    Private m_bMouseMState As Boolean = False
    '''<summary> マウス右ボタンの状態。 </summary>
    Private m_bMouseRState As Boolean = False

    '''<summary> ポップアップ表示文字列の内容。 </summary>
    Private m_PopupStr As String = Nothing
    '''<summary> ポップアップ表示文字列の点滅速度。 </summary>
    Private m_PopupBlink As Integer = 0

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   コンストラクタ／デストラクタ
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    'Protected Overridable Overloads Sub Dispose(disposing As Boolean)
    '   If disposing Then
    '       'dispose managed resources
    '       Me.picbox.Dispose()
    '   End If
    '   'free native resources
    'End Sub

    'Public Sub New(ByRef pb As PictureBox, ByRef fg As Label)
    '''<summary> 初期化を行う。 </summary>
    '''<param name="pb">描画先ピクチャーボックス。</param>
    '''<param name="fg">フォーカス取得用ラベル。</param>
    '''<remarks>フォーカス取得用ラベルは、グラフのホイール操作の際に使用します。</remarks>
    Public Sub New(ByRef pb As PictureBox, ByRef fg As Label, Optional ByVal pfMarge As System.Drawing.PointF = Nothing)
        '引数を保持
        Me.picbox = pb
        Me.FocusGetterLabel = fg
        '外枠範囲の初期値を指定
        '***************************************
        'MOD START M_UNO 20160818  Ver.2.0.1.3 統計画面調整
        'Me.GraphPosMin = New PointF(70.0F, 70.0F)
        'Me.GraphPosMax = New PointF(picbox.Width - 70.0F, picbox.Height - 70.0F)

        If IsNothing(pfMarge) = True Then
            Me.GraphPosMin = New PointF(70.0F, 70.0F)
        Else
            Me.GraphPosMin = New PointF(pfMarge.X, pfMarge.Y)
        End If
        Me.GraphPosMax = New PointF(picbox.Width - Me.GraphPosMin.X, picbox.Height - Me.GraphPosMin.Y)
        'MOD END M_UNO 20160818  Ver.2.0.1.3 統計画面調整
        '***************************************
        '表示範囲の初期値を格納
        Me.XAxis.MinVal = 0.0R
        Me.Y1Axis.MinVal = 0.0R
        Me.Y2Axis.MinVal = 0.0R
        Me.XAxis.MaxVal = 50.0R
        Me.Y1Axis.MaxVal = 50.0R
        Me.Y2Axis.MaxVal = 50.0R
        '変数初期化
        For iLCnt As Integer = 0 To MAX_LINE
            Me.m_GraphDP(iLCnt) = New PointF(MAX_DAT) {}
        Next iLCnt
    End Sub

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   コントロールイベント
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

#If FLAG_SHAREWARE = True Then

    'マウス押下時のイベント。
    Private Sub picbox_MouseDown(sender As Object, e As MouseEventArgs) Handles picbox.MouseDown
        'マウスの状態を表すメンバ変数をTrueにする
        If e.Button = Windows.Forms.MouseButtons.Left Then
            m_bMouseLState = True
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            m_bMouseRState = True
        End If
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            m_bMouseMState = True
        End If
        'If Me.FocusGetterLabel. = False Then
        'Me.FocusGetterLabel.Focus()
        Me.picbox.Select()
        'End If
    End Sub

    'マウス押上時のイベント。
    Private Sub picbox_MouseUp(sender As Object, e As MouseEventArgs) Handles picbox.MouseUp
        'マウスの状態を表すメンバ変数をFalseにする
        If e.Button = Windows.Forms.MouseButtons.Left Then
            m_bMouseLState = False
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            m_bMouseRState = False
        End If
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            m_bMouseMState = False
        End If
    End Sub

    'グラフドラッグ時の処理など。有償版のみ
    Private Sub picbox_MouseMove(sender As Object, e As MouseEventArgs) Handles picbox.MouseMove
        'グラフが小さすぎる場合の例外防止
        If GraphPosMax.X - GraphPosMin.X <= 1.0F OrElse GraphPosMax.Y - GraphPosMin.Y <= 1.0F Then
            Exit Sub
        End If

        '直前のマウス座標(PictureBox左上が原点)
        Static Dim iOldMouseX As Integer = 0
        Static Dim iOldMouseY As Integer = 0
        '右ドラッグ開始時のマウス座標(スクリーン左上が原点)
        Static Dim iDragStartX As Integer = 0
        Static Dim iDragStartY As Integer = 0
        '選択軸範囲
        Dim iCurrentSelAxis As Integer = -1
        'ドラッグ位置(スクリーン左上が原点)
        Static Dim ptDragPrev As System.Drawing.Point = New Point(0, 0)
        Static Dim ptDragStart As Point = New Point(0, 0)
        Static Dim ptPicDragStart As Point = New Point(0, 0)
        Dim ptCursor As Point = CType(sender, Control).PointToScreen(New Point(e.X, e.Y)) 'Control.MousePosition
        Dim ptDragEnd As Point = ptCursor
        '直前にドラッグ中であったかを示すフラグ
        Static Dim blDragFlag As Boolean = False
        'マウス左ボタンの処理
        If m_bMouseLState = True Then
            '左ドラッグによる操作を許可しない場合、処理を抜ける
            If bCanLeftDrag = False Then
                Exit Sub
            End If
            Dim sngStepX As Single = CSng(XAxis.MaxVal - XAxis.MinVal) / (GraphPosMax.X - GraphPosMin.X)
            Dim sngStepY As Single = CSng(Y1Axis.MaxVal - Y1Axis.MinVal) / (GraphPosMax.Y - GraphPosMin.Y)
            'マウス左クリックで表示位置をドラッグ移動
            XAxis.MinVal -= CSng(e.X - iOldMouseX) * sngStepX
            Y1Axis.MinVal += CSng(e.Y - iOldMouseY) * sngStepY
            XAxis.MaxVal -= CSng(e.X - iOldMouseX) * sngStepX
            Y1Axis.MaxVal += CSng(e.Y - iOldMouseY) * sngStepY
            sbLimitGraphPos()
            sbSetGraphPoints()
            sbRedrawGraph()
        Else
            'マウスの位置に対応する現在座標を表示
            '表示データ値の範囲を取得
            CursorPosXValue = (e.X - GraphPosMin.X) / (GraphPosMax.X - GraphPosMin.X) * (XAxis.MaxVal - XAxis.MinVal) + XAxis.MinVal
            CursorPosY1Value = (1.0F - (e.Y - GraphPosMin.Y) / (GraphPosMax.Y - GraphPosMin.Y)) * (Y1Axis.MaxVal - Y1Axis.MinVal) + Y1Axis.MinVal
            'm_dPBMouseX = sngCursorX
            'm_dPBMouseY1 = sngCursorY1
            If Y2Axis.Enabled = True Then
                CursorPosY2Value = (1.0F - (e.Y - GraphPosMin.Y) / (GraphPosMax.Y - GraphPosMin.Y)) * (Y2Axis.MaxVal - Y2Axis.MinVal) + Y2Axis.MinVal
            End If
        End If
        'マウス右クリックで表示位置を範囲指定
        If e.X < picbox.Width AndAlso e.X >= 0 Then
            If e.Y < picbox.Height AndAlso e.Y >= 0 Then
                'ドラッグ処理を許可しない場合、処理を抜ける
                If bCanRightDrag = False Then
                    Exit Sub
                End If
                If m_bMouseRState = True Then
                    'ドラッグ開始
                    If blDragFlag = False Then
                        ptDragStart = CType(sender, Control).PointToScreen(New Point(e.X, e.Y))
                        ptPicDragStart.X = e.X
                        ptPicDragStart.Y = e.Y
                        ControlPaint.DrawReversibleFrame(New Rectangle(ptDragStart.X, ptDragStart.Y, ptDragEnd.X - ptDragStart.X, ptDragEnd.Y - ptDragStart.Y),
                                                         Color.White, FrameStyle.Thick)
                    Else
                        ControlPaint.DrawReversibleFrame(New Rectangle(ptDragStart.X, ptDragStart.Y, ptDragEnd.X - ptDragStart.X, ptDragEnd.Y - ptDragStart.Y),
                                                         Color.White, FrameStyle.Thick)
                        ControlPaint.DrawReversibleFrame(New Rectangle(ptDragStart.X, ptDragStart.Y, ptDragPrev.X - ptDragStart.X, ptDragPrev.Y - ptDragStart.Y),
                                                         Color.White, FrameStyle.Thick)

                        Console.WriteLine(ptDragStart.X & "---" & ptDragEnd.X)
                    End If
                    ptDragPrev = ptDragEnd
                    blDragFlag = True
                Else
                    'ドラッグ終了
                    If blDragFlag = True Then
                        '拡大処理を実行
                        blDragFlag = False
                        ControlPaint.DrawReversibleFrame(New Rectangle(ptDragStart.X, ptDragStart.Y, ptDragPrev.X - ptDragStart.X, ptDragPrev.Y - ptDragStart.Y),
                                                         Color.White, FrameStyle.Thick)
                        '充分に広い範囲が選択されている場合、処理を続行
                        If Math.Abs(ptDragStart.X - ptDragEnd.X) >= 8 AndAlso Math.Abs(ptDragStart.Y - ptDragEnd.Y) >= 8 Then
                            'ドラッグ位置の表示グラフ上での範囲を取得（単位：軸幅）
                            Dim sngDragMinX As Single = (ptPicDragStart.X - GraphPosMin.X) / (GraphPosMax.X - GraphPosMin.X)
                            Dim sngDragMinY As Single = (ptPicDragStart.Y - GraphPosMin.Y) / (GraphPosMax.Y - GraphPosMin.Y)
                            Dim sngDragMaxX As Single = (e.X - GraphPosMin.X) / (GraphPosMax.X - GraphPosMin.X)
                            Dim sngDragMaxY As Single = (e.Y - GraphPosMin.Y) / (GraphPosMax.Y - GraphPosMin.Y)
                            '最小値と最大値が逆の場合、値を入れ替える
                            Dim sngSwapTemp As Single
                            If sngDragMinX > sngDragMaxX Then
                                sngSwapTemp = sngDragMinX
                                sngDragMinX = sngDragMaxX
                                sngDragMaxX = sngSwapTemp
                            End If
                            If sngDragMinY > sngDragMaxY Then
                                sngSwapTemp = sngDragMinY
                                sngDragMinY = sngDragMaxY
                                sngDragMaxY = sngSwapTemp
                            End If
                            '表示データ値の範囲を取得
                            Dim dDataRangeX As Double = XAxis.MaxVal - XAxis.MinVal
                            Dim dDataRangeY As Double = Y1Axis.MaxVal - Y1Axis.MinVal
                            '選択範囲の中央を取得
                            Dim dGraphCenterX As Double = CDbl((sngDragMaxX + sngDragMinX) / 2.0F) * dDataRangeX + XAxis.MinVal
                            Dim dGraphCenterY As Double = CDbl(1.0F - (sngDragMaxY + sngDragMinY) / 2.0F) * dDataRangeY + Y1Axis.MinVal
                            'ドラッグされた範囲に軸を設定
                            XAxis.MaxVal = CDbl(sngDragMaxX) * dDataRangeX + XAxis.MinVal
                            XAxis.MinVal = CDbl(sngDragMinX) * dDataRangeX + XAxis.MinVal
                            Y1Axis.MaxVal = CDbl(1.0F - sngDragMinY) * dDataRangeY + Y1Axis.MinVal
                            Y1Axis.MinVal = CDbl(1.0F - sngDragMaxY) * dDataRangeY + Y1Axis.MinVal
                            '表示幅が狭すぎる場合は最小値を設定し、マウス座標を中心とするように範囲を移動
                            sbExpandNarrowRange(dGraphCenterX, dGraphCenterY)
                            '表示範囲を調整後に再描画
                            Me.sbLimitGraphPos()
                            sbSetGraphPoints()
                            sbRedrawGraph()
                        End If
                    End If
                End If
            End If
        End If
        'マウスで指定されている軸を取得
        'X軸最小値
        If e.Y > GraphPosMax.Y Then
            If Math.Abs(GraphPosMin.X - e.X) <= 30.0F Then
                iCurrentSelAxis = 0
            End If
        End If
        'X軸最大値
        If e.Y > GraphPosMax.Y Then
            If Math.Abs(GraphPosMax.X - e.X) <= 30.0F Then
                iCurrentSelAxis = 1
            End If
        End If
        'Y1軸最小値
        If e.X < GraphPosMin.X Then
            If Math.Abs(GraphPosMax.Y - e.Y) <= 30.0F Then
                iCurrentSelAxis = 2
            End If
        End If
        'Y1軸最大値
        If e.X < GraphPosMin.X Then
            If Math.Abs(GraphPosMin.Y - e.Y) <= 30.0F Then
                iCurrentSelAxis = 3
            End If
        End If
        If Me.Y2Axis.Enabled Then
            'Y2軸最小値
            If e.X > GraphPosMax.X Then
                If Math.Abs(GraphPosMax.Y - e.Y) <= 30.0F Then
                    iCurrentSelAxis = 4
                End If
            End If
            'Y2軸最大値
            If e.X > GraphPosMax.X Then
                If Math.Abs(GraphPosMin.Y - e.Y) <= 30.0F Then
                    iCurrentSelAxis = 5
                End If
            End If
        End If
        '現在のマウス位置を取得しておく
        iOldMouseX = e.X
        iOldMouseY = e.Y
    End Sub

#End If

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   パブリック関数
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '============================================
    '軸のオートスケールを実行してグラフ再描画を行う
    '引数	なし
    '戻り値	なし
    '============================================
    '''<summary> 軸のオートスケールを実行して再描画する。 </summary>
    Public Sub FitScale()
        'グラフの表示範囲取得
        sbAutoFitAxis()
        'グラフ再描画
        sbRedrawGraph()
    End Sub

    '============================================
    'ポップアップ文字をグラフに表示
    '引数	strDraw:	描画する文字列
    '		iBlink:		文字の点滅間隔
    '戻り値	なし
    '============================================
    '''<summary> ポップアップ文字をグラフに表示する。 </summary>
    '''<param name="strDraw">描画する文字列。</param>
    '''<param name="iBlink">文字の点滅間隔。</param>
    Public Sub DrawPopup(ByVal strDraw As String, ByVal iBlink As Integer)
        '***************************************
        'MOD START M_UNO 20150701 Ver.1.0.3.7 ソースを見やすく修正
        '	'文字列が指定されている場合
        '	If IsNothing(strDraw) = False Then
        '		'空文字でない場合
        '		If strDraw <> "" Then
        '			'ポップアップ表示文字列の更新
        '			m_PopupStr = strDraw
        '		Else
        '			'ポップアップ表示無し
        '			m_PopupStr = Nothing
        '		End If
        '	Else
        '		'ポップアップ表示無し
        '		m_PopupStr = Nothing
        '	End If

        '文字列長が0の場合、NULLとする
        If strDraw.Length = 0 Then
            strDraw = Nothing
        End If
        'ポップアップ文字列の更新
        m_PopupStr = strDraw
        'MOD END M_UNO 20150701 Ver.1.0.3.7 ソースを見やすく修正
        '***************************************

        'ポップアップ文字の点滅間隔を設定
        m_PopupBlink = iBlink

        'グラフ再描画
        sbRedrawGraph()
    End Sub

    '============================================
    '表示データの追加
    '引数	dX:			X軸データ配列
    '		dY:			Y軸データ配列
    '		dS:			各点に対応する文字列
    '		strkey:		データに対応する文字列
    '戻り値	追加Index
    '============================================
    '''<summary> 表示データを追加する。 </summary>
    '''<param name="dX">データX値の配列。</param>
    '''<param name="dY">データY値の配列。</param>
    '''<param name="dS">各点に対応する文字列の配列。</param>
    '''<param name="strkey">データに対応する文字列。</param>
    '''<returns>追加データに対応するData配列のインデックス値。</returns>
    '''<remarks>返り値は変数に代入して、線種変更などの際のアクセスに使用してください。</remarks>
    Public Function AddData(ByVal dX As Double(), ByVal dY As Double(), ByVal dS As String(), ByVal strkey As String) As Integer
        Dim iNewDE As Integer

        'グラフに表示するデータが空の場合
        If IsNothing(Data) Then
            iNewDE = 0
        Else
            '空データでないデータ数を取得
            iNewDE = Data.Count(Function(f) IsNothing(f) = False)
        End If

		Data(iNewDE) = New FCData With {
			.X = dX,
			.Y = dY
		}
		If IsNothing(dS) = False Then
            Data(iNewDE).S = dS
        End If
        Dim dXM As Integer = dX.Count - 1
        Data(iNewDE).Bld = New Boolean(dXM) {}
        For iCnt As Integer = 0 To dXM
            Data(iNewDE).Bld(iCnt) = False
        Next iCnt
        Data(iNewDE).Key = strkey
        Return iNewDE
    End Function

    '''<summary> 表示データの内容を更新する。 </summary>
    '''<param name="iDE">対応するデータ配列の添え字。</param>
    '''<param name="dX">データX値の配列。</param>
    '''<param name="dY">データY値の配列。</param>
    '''<param name="dS">各点に対応する文字列の配列。</param>
    '''<returns>更新に成功した場合True。</returns>
    '''<remarks>iDEは、AddDataの返り値で取得したインデックス番号が対応します。</remarks>
    Public Function RefreshData(ByVal iDE As Integer, ByVal dX As Double(), ByVal dY As Double(), ByVal dS As String()) As Boolean
        If IsNothing(Data(iDE)) Then
            Return False
        End If
        If iDE < Data.Count AndAlso iDE >= 0 Then
			Data(iDE) = New FCData With {
				.X = dX,
				.Y = dY
			}
			If IsNothing(dS) = False Then
                Data(iDE).S = dS
            End If
            Dim dXM As Integer = dX.Count - 1
            Data(iDE).Bld = New Boolean(dXM) {}
            For iCnt As Integer = 0 To dXM
                Data(iDE).Bld(iCnt) = False
            Next iCnt
            Return True
        End If
        Return False
    End Function

    '表示データ削除関数。未実装。
    'Public Function DeleteData(ByVal iDE As Integer) As Boolean
    '   If IsNothing(Data(iDE)) Then
    '       Return False
    '   End If
    '   Data(iDE) = New FCData()
    '   Return True
    'End Function

    '''<summary> 表示データをクリアする。 </summary>
    Public Function ClearData() As Boolean
        Data = New FCData(MAX_LINE) {}
        Return True
    End Function

    '''<summary> 再描画を行う。 </summary>
    Public Sub Redraw()
        sbSetGraphPoints()
        sbRedrawGraph()

        '***************************************
        'ADD START M_UNO 20170314 Ver.--- 相関グラフ
        '残像が残る？
        picbox.Refresh()
        'ADD END M_UNO 20170314 Ver.--- 相関グラフ
        '***************************************
    End Sub

#If FLAG_SHAREWARE = True Then

    '''<summary> グラフの拡大縮小処理を行う。 </summary>
    '''<param name="dZoomX">X軸のズーム率。1より小さい値で拡大、1より大きい値で縮小。</param>
    '''<param name="dZoomY">Y軸のズーム率。1より小さい値で拡大、1より大きい値で縮小。</param>
    Public Sub ZoomChange(ByVal dZoomX As Double, ByVal dZoomY As Double)
        'グラフを中心に拡大縮小処理を行う
        Dim dScaleX As Double
        Dim dScaleY As Double
        Dim dPrevScaleX As Double
        Dim dPrevScaleY As Double
        dPrevScaleX = XAxis.MaxVal - XAxis.MinVal
        dPrevScaleY = Y1Axis.MaxVal - Y1Axis.MinVal
        dScaleX = dPrevScaleX * dZoomX
        dScaleY = dPrevScaleY * dZoomY
        XAxis.MinVal = XAxis.MinVal + ((dPrevScaleX - dScaleX) / 2.0R)
        XAxis.MaxVal = XAxis.MaxVal - ((dPrevScaleX - dScaleX) / 2.0R)
        Y1Axis.MinVal = Y1Axis.MinVal + ((dPrevScaleY - dScaleY) / 2.0R)
        Y1Axis.MaxVal = Y1Axis.MaxVal - ((dPrevScaleY - dScaleY) / 2.0R)
        'グラフ中央を取得
        Dim dGraphCenterX As Double = (XAxis.MaxVal + XAxis.MinVal) / 2.0R
        Dim dGraphCenterY As Double = (Y1Axis.MaxVal + Y1Axis.MinVal) / 2.0R
        '表示幅が狭すぎる場合は最小値を設定し、マウス座標を中心とするように範囲を移動
        'Me.sbExpandNarrowRange(dGraphCenterX, dGraphCenterY)
        'Me.sbLimitGraphPos()
        sbSetGraphPoints()
        sbRedrawGraph()
		''グラフ縮小
		'If e.Delta < 0 Then
		'	dScaleX = dPrevScaleX * L_ZM_OUT
		'	dScaleY = dPrevScaleY * L_ZM_OUT
		'	XAxis.MinVal = XAxis.MinVal + ((dPrevScaleX - dScaleX) / 2.0R)
		'	XAxis.MaxVal = XAxis.MaxVal - ((dPrevScaleX - dScaleX) / 2.0R)
		'	Y1Axis.MinVal = Y1Axis.MinVal + ((dPrevScaleY - dScaleY) / 2.0R)
		'	Y1Axis.MaxVal = Y1Axis.MaxVal - ((dPrevScaleY - dScaleY) / 2.0R)
		'	'グラフ中央を取得
		'	Dim dGraphCenterX As Double = (XAxis.MaxVal + XAxis.MinVal) / 2.0R
		'	Dim dGraphCenterY As Double = (Y1Axis.MaxVal + Y1Axis.MinVal) / 2.0R
		'	'表示幅が狭すぎる場合は最小値を設定し、マウス座標を中心とするように範囲を移動
		'	Me.sbExpandNarrowRange(dGraphCenterX, dGraphCenterY)
		'	Me.sbLimitGraphPos()
		'	sbSetGraphPoints()
		'	sbRedrawGraph()
		'End If
	End Sub

    '''<summary> グラフ中でクリックされた点に対応する文字列Sを取得する。 </summary>
    '''<param name="iMouseX">クリックされたpicbox上のX座標。</param>
    '''<param name="iMouseY">クリックされたpicbox上のY座標。</param>
    '''<param name="DispSelected">Trueの場合、選択された点を強調表示する。</param>
    Public Function GetClickedPoint(ByVal iMouseX As Integer, ByVal iMouseY As Integer, ByVal DispSelected As Boolean) As String
        sbSetGraphPoints()
        '描画用座標
        Dim ptDraw() As PointF
        ptDraw = New PointF(MAX_DAT) {}
        '描画用座標をピクチャボックスサイズに応じて設定
        Dim sngGraphSizeX As Single = GraphPosMax.X - GraphPosMin.X
        Dim sngGraphSizeY As Single = GraphPosMax.Y - GraphPosMin.Y
        'ビットマップオブジェクト
        Dim bmGraph As Bitmap = New Bitmap(picbox.Width, picbox.Height)
        'グラフィックオブジェクト
        Dim grpGraph As Graphics = Graphics.FromImage(bmGraph)
        '背景を白色でクリア
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0.0F, 0.0F, picbox.Width, picbox.Height))
        'グリッドは描画しない
        '指定本数だけグラフを描画
        Dim iLMax As Integer = Me.Data.Count(Function(f) IsNothing(f) = False) - 1
        For iLCnt = 0 To iLMax
            '測定データの処理
            Dim iDCV As Integer = Data(iLCnt).X.Count - 1
            ptDraw = New PointF(iDCV) {}
            For i As Integer = 0 To iDCV Step 1
                ptDraw(i).X = m_GraphDP(iLCnt)(i).X * sngGraphSizeX + GraphPosMin.X
                ptDraw(i).Y = m_GraphDP(iLCnt)(i).Y * sngGraphSizeY + GraphPosMin.Y
            Next i
            'データ内容を判定レイヤに描画、緑と青に線番号を代入
            If Data(iLCnt).Key <> "" Then
                Try
                    'データ内容を描画
                    Select Case Data(iLCnt).DrawType
                        'Case 0
                        ''線
                        'Dim penCheck As Pen = New Pen(Color.FromArgb(255, iLCnt, 0, 0), 10.0F)
                        'If Data(iLCnt).Selected = False Then
                        '   grpGraph.DrawLines(penCheck, ptDraw)
                        'Else
                        '   Dim penSel As Pen = New Pen(Data(iLCnt).Pen.Color, 5.0F)
                        '   grpGraph.DrawLines(penSel, ptDraw)
                        'End If
                        Case 1
                            '点
                            'Dim penCheck As Pen = New Pen(Color.FromArgb(255, 0, iLCnt / 256, iLCnt Mod 256), 10.0F)
                            Dim brs As Brush
                            Dim iPCMax As Integer = Data(iLCnt).X.Count - 1
                            '最大数を65535に制限。
                            If iPCMax > 65535 - 1 Then
                                iPCMax = 65535 - 1
                            End If
                            For iPCnt = 0 To iPCMax
                                If IsNothing(Data(iLCnt).S(iPCnt)) = False Then
                                    Dim iPtMax As Integer = ptDraw.Count - 1
                                    Data(iLCnt).Bld(iPCnt) = False
                                    brs = New SolidBrush(Color.FromArgb(255, iLCnt, Math.Floor(CDbl(iPCnt) / 256.0R), iPCnt Mod 256))
                                    grpGraph.FillEllipse(brs, ptDraw(iPCnt).X - 11.0F, ptDraw(iPCnt).Y - 11.0F, 22.0F, 22.0F)
                                End If
                            Next iPCnt
                            'Case 2
                            '   '両方
                            '   Dim penCheck As Pen = New Pen(Color.FromArgb(255, iLCnt, 0, 0), 10.0F)
                            '   grpGraph.DrawLines(penCheck, ptDraw)
                    End Select
                Catch ex As OverflowException
                    'オーバーフロー時は描画しない。
                    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
                End Try
            End If
        Next iLCnt
        'グラフ外を白色で塗りなおす
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0, 0, picbox.Width, GraphPosMin.Y))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0, 0, GraphPosMin.X, picbox.Height))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(GraphPosMax.X + 1, 0, picbox.Width, picbox.Height))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0, GraphPosMax.Y + 1, picbox.Width, picbox.Height))
        'ピクセルの色を取得
        Dim colorPix As Color
        If iMouseX >= 0 AndAlso iMouseX < picbox.Width AndAlso iMouseY >= 0 AndAlso iMouseY < picbox.Height Then
            colorPix = bmGraph.GetPixel(iMouseX, iMouseY)
        Else
            Return -1
        End If
        'リソースを解放
        grpGraph.Dispose()
        '選択強調表示
        Dim iIdx As Integer = CInt(colorPix.G) * 256 + CInt(colorPix.B)
        If colorPix.R >= 255 Then
            Return ""
        End If
        If DispSelected = True Then
            Data(colorPix.R).Bld(iIdx) = True   '選択された点の表示を強調する
        End If
        'クリックされた点データに対応する文字列Sを返す
        Return Data(colorPix.R).S(iIdx)
    End Function

    '''<summary> グラフの表示範囲が引数を超えている場合、表示範囲を移動する。 </summary>
    Public Overloads Sub sbLimitGraphPos(ByVal dMinX As Double, ByVal dMaxX As Double,
                                ByVal dMinY1 As Double, ByVal dMaxY1 As Double,
                                ByVal dMinY2 As Double, ByVal dMaxY2 As Double)
        If Me.XAxis.MinVal < dMinX Then
            Me.XAxis.MaxVal += dMinX - Me.XAxis.MinVal
            Me.XAxis.MinVal = dMinX
        End If
        If Me.XAxis.MaxVal > dMaxX Then
            Me.XAxis.MinVal -= Me.XAxis.MaxVal - dMaxX
            Me.XAxis.MaxVal = dMaxX
            If Me.XAxis.MinVal < dMinX Then
                Me.XAxis.MinVal = dMinX
            End If
        End If
        If Me.Y1Axis.MinVal < dMinY1 Then
            Me.Y1Axis.MaxVal += dMinY1 - Me.Y1Axis.MinVal
            Me.Y1Axis.MinVal = dMinY1
        End If
        If Me.Y1Axis.MaxVal > dMaxY1 Then
            Me.Y1Axis.MinVal -= Me.Y1Axis.MaxVal - dMaxY1
            Me.Y1Axis.MaxVal = dMaxY1
            If Me.Y1Axis.MinVal < dMinY1 Then
                Me.Y1Axis.MinVal = dMinY1
            End If
        End If
    End Sub

    '''<summary> グラフの表示範囲が軸の取りえる範囲を超えている場合、表示範囲を移動する。 </summary>
    Public Overloads Sub sbLimitGraphPos()
        sbLimitGraphPos(XAxis.LowestVal, XAxis.HighestVal, Y1Axis.LowestVal, Y1Axis.HighestVal, Y2Axis.LowestVal, Y2Axis.HighestVal)
    End Sub

    '''<summary> グラフの表示範囲が狭すぎる場合、拡大する。 </summary>
    Public Overloads Sub sbExpandNarrowRange(ByVal dCenterX As Double, ByVal dCenterY1 As Double)
        If Math.Abs(XAxis.MaxVal - XAxis.MinVal) < XAxis.NarrowestRange Then
            XAxis.MaxVal = dCenterX + XAxis.NarrowestRange / 2.0R
            XAxis.MinVal = dCenterX - XAxis.NarrowestRange / 2.0R
        End If
        If Math.Abs(Y1Axis.MaxVal - Y1Axis.MinVal) < Y1Axis.NarrowestRange Then
            Y1Axis.MaxVal = dCenterY1 + Y1Axis.NarrowestRange / 2.0R
            Y1Axis.MinVal = dCenterY1 - Y1Axis.NarrowestRange / 2.0R
        End If
    End Sub
    '''<summary> グラフの表示範囲が狭すぎる場合、拡大する。 </summary>
    Public Overloads Sub sbExpandNarrowRange(ByVal dCenterX As Double, ByVal dCenterY1 As Double, ByVal dCenterY2 As Double)
        If Math.Abs(XAxis.MaxVal - XAxis.MinVal) < XAxis.NarrowestRange Then
            XAxis.MaxVal = dCenterX + XAxis.NarrowestRange / 2.0R
            XAxis.MinVal = dCenterX - XAxis.NarrowestRange / 2.0R
        End If
        If Math.Abs(Y1Axis.MaxVal - Y1Axis.MinVal) < Y1Axis.NarrowestRange Then
            Y1Axis.MaxVal = dCenterY1 + Y1Axis.NarrowestRange / 2.0R
            Y1Axis.MinVal = dCenterY1 - Y1Axis.NarrowestRange / 2.0R
        End If
        If Math.Abs(Y2Axis.MaxVal - Y2Axis.MinVal) < Y2Axis.NarrowestRange Then
            Y2Axis.MaxVal = dCenterY2 + Y2Axis.NarrowestRange / 2.0R
            Y2Axis.MinVal = dCenterY2 - Y2Axis.NarrowestRange / 2.0R
        End If
    End Sub

#End If

    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    '   プライベート関数
    '■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    '============================================
    'グラフ画面表示用座標をデータ座標から設定する
    '引数	なし
    '戻り値	なし
    '============================================
    '''<summary> グラフ画面表示用座標をデータ座標から設定する。 </summary>
    Private Sub sbSetGraphPoints()
        If IsNothing(Me.Data) Then
            Exit Sub
        End If
        Dim iLMax As Integer = Me.Data.Count(Function(f) IsNothing(f) = False) - 1
        '負の値表示を行わない場合に表示範囲を制限
        Dim XRange As Double = (XAxis.MaxVal - XAxis.MinVal)
        Dim Y1Range As Double = (Y1Axis.MaxVal - Y1Axis.MinVal)
        Dim Y2Range As Double = (Y2Axis.MaxVal - Y2Axis.MinVal)
        'グラフ画面表示用座標を設定。
        'Xは左軸を0.0、右軸を1.0, Y軸は上軸を0.0, 右軸を1.0とする
        For iLCnt As Integer = 0 To iLMax
            If IsNothing(Me.Data(iLCnt)) Then
                Continue For
            End If
            If Data(iLCnt).X.Count = 0 Then
                Continue For
            End If
            m_GraphDP(iLCnt)(0).X = CSng((Data(iLCnt).X(0) - XAxis.MinVal) / XRange)
            If Data(iLCnt).RefAxis = 0 Then
                m_GraphDP(iLCnt)(0).Y = 1.0F - CSng((Data(iLCnt).Y(0) - Y1Axis.MinVal) / Y1Range)
            ElseIf Data(iLCnt).RefAxis = 1 Then
                m_GraphDP(iLCnt)(0).Y = 1.0F - CSng((Data(iLCnt).Y(0) - Y2Axis.MinVal) / Y2Range)
            End If
            Dim iDC As Integer = Data(iLCnt).X.Count
            If Data(iLCnt).RefAxis = 0 Then
                'Y1軸を参照する場合の処理
                For i As Integer = 1 To iDC - 1 Step 1
                    m_GraphDP(iLCnt)(i).X = CSng((Data(iLCnt).X(i) - XAxis.MinVal) / XRange)
                    m_GraphDP(iLCnt)(i).Y = 1.0F - CSng((Data(iLCnt).Y(i) - Y1Axis.MinVal) / Y1Range)
                Next i
            ElseIf Data(iLCnt).RefAxis = 1 Then
                'Y2軸を参照する場合の処理
                For i As Integer = 1 To iDC - 1 Step 1
                    m_GraphDP(iLCnt)(i).X = CSng((Data(iLCnt).X(i) - XAxis.MinVal) / XRange)
                    m_GraphDP(iLCnt)(i).Y = 1.0F - CSng((Data(iLCnt).Y(i) - Y2Axis.MinVal) / Y2Range)
                Next i
            End If
        Next iLCnt
    End Sub

    '============================================
    'グラフの表示範囲を現在データから取得して設定
    '引数	なし
    '戻り値	なし
    '============================================
    '''<summary>
    '''グラフの表示範囲を現在のデータから取得して設定する。
    '''再描画は行わない。
    '''</summary>
    Private Sub sbAutoFitAxis()
        '***************************************
        'ADD START M_UNO 20170314 Ver.--- 相関グラフ
        If Data Is Nothing OrElse Data.Length = 0 OrElse IsNothing(Data(0)) = True Then
            Exit Sub
        End If
        'ADD END M_UNO 20170314 Ver.--- 相関グラフ
        '***************************************

        Dim dMinX As Double
        Dim dMaxX As Double
        Dim dMinY1 As Double
        Dim dMaxY1 As Double
        Dim dMinY2 As Double
        Dim dMaxY2 As Double
        XAxis.MinVal = Data(0).X.Min()
        XAxis.MaxVal = Data(0).X.Max()
        If Data(0).RefAxis = 0 Then
            Y1Axis.MinVal = Data(0).Y.Min()
            Y1Axis.MaxVal = Data(0).Y.Max()
            Y2Axis.MinVal = Double.PositiveInfinity
            Y2Axis.MaxVal = Double.NegativeInfinity
        Else
            Y1Axis.MinVal = Double.PositiveInfinity
            Y1Axis.MaxVal = Double.NegativeInfinity
            Y2Axis.MinVal = Data(0).Y.Min()
            Y2Axis.MaxVal = Data(0).Y.Max()
        End If
        Dim iDC As Integer = Data.Count(Function(f) IsNothing(f) = False) - 1
        For i As Integer = 1 To iDC
            If Data(i).Kind = 0 Then
                If Data(i).RefAxis = 0 Then
                    'Y1軸の場合
                    dMinX = Data(i).X.Min()
                    If XAxis.MinVal > dMinX Then
                        XAxis.MinVal = dMinX
                    End If
                    dMaxX = Data(i).X.Max()
                    If XAxis.MaxVal < dMaxX Then
                        XAxis.MaxVal = dMaxX
                    End If
                    dMinY1 = Data(i).Y.Min()
                    If Y1Axis.MinVal > dMinY1 Then
                        Y1Axis.MinVal = dMinY1
                    End If
                    dMaxY1 = Data(i).Y.Max()
                    If Y1Axis.MaxVal < dMaxY1 Then
                        Y1Axis.MaxVal = dMaxY1
                    End If
                Else
                    'Y2軸の場合
                    dMinX = Data(i).X.Min()
                    If XAxis.MinVal > dMinX Then
                        XAxis.MinVal = dMinX
                    End If
                    dMaxX = Data(i).X.Max()
                    If XAxis.MaxVal < dMaxX Then
                        XAxis.MaxVal = dMaxX
                    End If
                    dMinY2 = Data(i).Y.Min()
                    If Y2Axis.MinVal > dMinY2 Then
                        Y2Axis.MinVal = dMinY2
                    End If
                    dMaxY2 = Data(i).Y.Max()
                    If Y2Axis.MaxVal < dMaxY2 Then
                        Y2Axis.MaxVal = dMaxY2
                    End If
                End If
            End If
        Next i
        '最小の範囲を設定する。
        sbSetGraphMinimumRange()
    End Sub

    '============================================
    'グラフの最小表示範囲を設定
    '引数	なし
    '戻り値	なし
    '============================================
    '''<summary> グラフの最小表示範囲を設定する。 </summary>
    Private Sub sbSetGraphMinimumRange()
        If XAxis.Kind = 0 Then
            If XAxis.MinVal >= XAxis.MaxVal Then
                XAxis.MaxVal = XAxis.MinVal + 1.0R
            End If
        Else
            If XAxis.MinVal >= XAxis.MaxVal Then
                XAxis.MaxVal = XAxis.MinVal + 1000000000.0R
            End If
        End If
        If Double.IsPositiveInfinity(Y1Axis.MinVal) Then
            Y1Axis.MinVal = 0.0R
        End If
        If Y1Axis.MinVal >= Y1Axis.MaxVal Then
            Y1Axis.MaxVal = Y1Axis.MinVal + 1.0R
        End If
        If Data.Count > 1 Then
            If Double.IsPositiveInfinity(Y2Axis.MinVal) Then
                Y2Axis.MinVal = 0.0R
            End If
            If Y2Axis.MinVal >= Y2Axis.MaxVal Then
                Y2Axis.MaxVal = Y2Axis.MinVal + 1.0R
            End If
        Else
            Y2Axis.MinVal = 0.0R
            Y2Axis.MaxVal = 1.0R
        End If
    End Sub

    '============================================
    'グラフ再描画
    '引数	なし
    '戻り値	なし
    '============================================
    '''<summary>
    '''グラフ画面表示用座標を参照して、グラフを再描画する。
    '''</summary>
    Private Sub sbRedrawGraph()
        'PictureBoxが小さすぎる場合に処理を中断する
        If picbox.Width <= 30 OrElse picbox.Height <= 30 Then
            Exit Sub
        End If
        'レイアウト変更を一時停止
        picbox.SuspendLayout()
        'Me.SuspendLayout()

        '描画用座標をピクチャボックスサイズに応じて設定
        Dim sngGraphSizeX As Single = GraphPosMax.X - GraphPosMin.X
        Dim sngGraphSizeY As Single = GraphPosMax.Y - GraphPosMin.Y

        'ビットマップオブジェクト。画像サイズが変更された場合のみ新規インスタンスを作成
        Static Dim bmGraph As Bitmap = New Bitmap(picbox.Width, picbox.Height)
        If bmGraph.Width <> picbox.Width OrElse bmGraph.Height <> picbox.Height Then
            bmGraph = New Bitmap(picbox.Width, picbox.Height)
        End If
        'グラフィックオブジェクト
        Dim grpGraph As Graphics = Graphics.FromImage(bmGraph)
        'Dim grpGraph As Graphics = pbGraph.CreateGraphics()
        '背景をクリア
        grpGraph.FillRectangle(BGBrush, New Rectangle(0, 0, picbox.Width, picbox.Height))
        'グリッドを描画
        sbDrawGridLine(grpGraph, sngGraphSizeX, sngGraphSizeY)
        'データを描画
        sbDrawData(grpGraph, sngGraphSizeX, sngGraphSizeY)

        '***************************************
        'MOD START M_UNO 20160805  Ver.2.0.1.2  縦軸タイトル追加
        'Static Dim axisfont As Font = New Font("MS Gothic", 14)
        'Static Dim axisfont As Font = axisfont
        'MOD END M_UNO 20160805  Ver.2.0.1.2  縦軸タイトル追加
        '***************************************
        Static Dim axisfontsml As Font = New Font("MS Gothic", 10)

        'グラフ外を白色で塗りなおす
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0.0F, 0.0F, picbox.Width, GraphPosMin.Y))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0.0F, 0.0F, GraphPosMin.X, picbox.Height))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(GraphPosMax.X + 1.0F, 0.0F, picbox.Width, picbox.Height))
        grpGraph.FillRectangle(Brushes.White, New Rectangle(0.0F, GraphPosMax.Y + 1.0F, picbox.Width, picbox.Height))
        'グリッドごとの値を描画
        sbDrawGridValue(grpGraph, sngGraphSizeX, sngGraphSizeY, axisfontsml)
        'ポップアップ表示があれば行う
        Static Dim iLoopFlag As Integer = 0
        If IsNothing(m_PopupStr) = False Then
            Dim fntPopup As Font = New Font("MS Gothic", picbox.Width / 50.0F)
            grpGraph.FillRectangle(Brushes.Orange, New Rectangle(GraphPosMin.X + (GraphPosMax.X - GraphPosMin.X) * 1.0F / 4.0F, picbox.Height / 2.0F - 20.0F,
                                                                 (GraphPosMax.X - GraphPosMin.X) * 1.0F / 2.0F, 40.0F))
			Dim sf As StringFormat = New StringFormat With {
				.LineAlignment = StringAlignment.Center,
				.Alignment = StringAlignment.Center
			}
			If m_PopupBlink > 0 Then
                'm_PopupBlinkが正の値の場合、文字を点滅表示
                If iLoopFlag <= m_PopupBlink Then
                    grpGraph.DrawString(m_PopupStr, fntPopup, Brushes.Black,
                                         picbox.Width / 2.0F, picbox.Height / 2.0F, sf)
                Else
                    If iLoopFlag >= m_PopupBlink * 2 Then
                        iLoopFlag = 0
                    End If
                End If
            Else
                'm_PopupBlinkが0以下の場合、文字を常に表示
                grpGraph.DrawString(m_PopupStr, fntPopup, Brushes.Black,
                                     picbox.Width / 2.0F, picbox.Height / 2.0F, sf)
            End If
            iLoopFlag += 1
        End If

		'軸名を描画
		Dim sfa As StringFormat = New StringFormat With {
			.LineAlignment = StringAlignment.Center,
			.Alignment = StringAlignment.Center
		}

			'***************************************
			'MOD START M_UNO 20170314 Ver.--- 相関グラフ
			'grpGraph.DrawString(XAxis.Text, axisfont, Brushes.Black,
			'                    picbox.Width / 2.0F, GraphPosMax.Y + 35, sfa)

			'描画サイズを取得
		Dim sfSize As SizeF = grpGraph.MeasureString(XAxis.Text, axisfont, picbox.Width, sfa)
        grpGraph.DrawString(XAxis.Text, axisfont, Brushes.Black,
                            picbox.Width / 2.0F, picbox.Height - (sfSize.Height / 2), sfa)
        'MOD END M_UNO 20170314 Ver.--- 相関グラフ
        '***************************************
        Dim strAxis As String
        strAxis = Y1Axis.Text
        grpGraph.TranslateTransform(15.0F, picbox.Height / 2)
        grpGraph.RotateTransform(-90.0F)
        grpGraph.DrawString(strAxis, axisfont, Brushes.Black,
                            0, 0, sfa)
        grpGraph.ResetTransform()
        If Y2Axis.Enabled = True Then
            strAxis = Y2Axis.Text
            grpGraph.TranslateTransform(picbox.Width - 15.0F, picbox.Height / 2)
            grpGraph.RotateTransform(90.0F)
            grpGraph.DrawString(strAxis, axisfont, Brushes.Black,
                                0, 0, sfa)
            grpGraph.ResetTransform()
        End If

        '左上文字表示
        If strLT <> "" AndAlso bDispStrs = True Then
            sfa.LineAlignment = StringAlignment.Near
            sfa.Alignment = StringAlignment.Near
            grpGraph.DrawString(strLT, axisfontsml, Brushes.Black, 6.0F, 6.0F, sfa)
        End If
        '右下文字表示
        If strRB <> "" AndAlso bDispStrs = True Then
            '***************************************
            'MOD START M_UNO 20160818  Ver.2.0.1.3 統計画面調整
            'sfa.LineAlignment = StringAlignment.Far
            'sfa.Alignment = StringAlignment.Far
            'grpGraph.DrawString(strRB, axisfontsml, Brushes.Black, picbox.Width - 6.0F, picbox.Height - 6.0F, sfa)
            sfa.LineAlignment = StringAlignment.Near
            sfa.Alignment = StringAlignment.Far
            grpGraph.DrawString(strRB, axisfontsml, Brushes.Black, picbox.Width - 6.0F, 6.0F, sfa)
            'MOD END M_UNO 20160818  Ver.2.0.1.3 統計画面調整
            '***************************************
        End If

        'ピクチャボックスに反映
        picbox.Image = bmGraph

        'レイアウト変更を再開
        'picbox.ResumeLayout()
        picbox.Refresh()
        'リソースを解放
        grpGraph.Dispose()
    End Sub

    '============================================
    'グリッド描画
    '引数	grpGraph:		Graphicsオブジェクト
    '		sngGraphSizeX:	グラフ描画サイズ(X軸)
    '		sngGraphSizeY:	グラフ描画サイズ(Y軸)
    '戻り値	なし
    '============================================
    '''<summary>グリッドを描画する。</summary>(呼出元:sbRedrawGraph)
    Private Sub sbDrawGridLine(ByRef grpGraph As Graphics, ByVal sngGraphSizeX As Single, ByVal sngGraphSizeY As Single)
        If GridPen.DashStyle <> Drawing2D.DashStyle.Custom Then
            For i As Integer = 1 To 10
                '縦線
                grpGraph.DrawLine(GridPen, GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMin.Y,
                                    GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMax.Y)
                '横線
                grpGraph.DrawLine(GridPen, GraphPosMin.X, GraphPosMin.Y + (sngGraphSizeY * i) / 10,
                                    GraphPosMax.X, GraphPosMin.Y + (sngGraphSizeY * i) / 10)
            Next i
        End If
        '外枠線を描画
        grpGraph.DrawRectangle(Pens.Black, New Rectangle(GraphPosMin.X, GraphPosMin.Y,
                                                         GraphPosMax.X - GraphPosMin.X,
                                                         GraphPosMax.Y - GraphPosMin.Y))
        '目盛り線を描画
        For i As Integer = 1 To 10
            '上枠
            grpGraph.DrawLine(Pens.Black, GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMin.Y,
                              GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMin.Y + 2 + ((i + 1) Mod 2) * 2)
            '下枠
            grpGraph.DrawLine(Pens.Black, GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMax.Y - 2 - ((i + 1) Mod 2) * 2,
                              GraphPosMin.X + (sngGraphSizeX * i) / 10, GraphPosMax.Y)
            '左枠
            grpGraph.DrawLine(Pens.Black, GraphPosMin.X, GraphPosMin.Y + (sngGraphSizeY * i) / 10,
                              GraphPosMin.X + 2 + ((i + 1) Mod 2) * 2, GraphPosMin.Y + (sngGraphSizeY * i) / 10)
            '右枠
            grpGraph.DrawLine(Pens.Black, GraphPosMax.X - 2 - ((i + 1) Mod 2) * 2, GraphPosMin.Y + (sngGraphSizeY * i) / 10,
                              GraphPosMax.X, GraphPosMin.Y + (sngGraphSizeY * i) / 10)
        Next i
    End Sub

    '============================================
    'データ店描画
    '引数	grpGraph:		Graphicsオブジェクト
    '		sngGraphSizeX:	グラフ描画サイズ(X軸)
    '		sngGraphSizeY:	グラフ描画サイズ(Y軸)
    '戻り値	なし
    '============================================
    '''<summary>データ点を描画する。</summary>(呼出元:sbRedrawGraph)
    Private Sub sbDrawData(ByRef grpGraph As Graphics, ByVal sngGraphSizeX As Single, ByVal sngGraphSizeY As Single)
        If Not IsNothing(Me.Data) Then
            '指定本数だけグラフを描画
            Dim iDC As Integer = Data.Count(Function(f) IsNothing(f) = False) - 1
            '描画用座標
            Dim ptDraw() As PointF '= New PointF(1) {}
            '線の数だけ繰り返し処理
            For iLCnt = 0 To iDC
                If IsNothing(Me.Data(iLCnt)) Then
                    Continue For
                End If
                'データ点数
                Dim iDCV As Integer = Data(iLCnt).X.Count - 1
                'データをプロット用変数に代入。グラフの荒さRoughを考慮する
                If iDCV <= Me.Rough Then
                    ptDraw = New PointF(iDCV) {}
                    For i As Integer = 0 To iDCV Step 1
                        ptDraw(i).X = m_GraphDP(iLCnt)(i).X * sngGraphSizeX + GraphPosMin.X
                        ptDraw(i).Y = m_GraphDP(iLCnt)(i).Y * sngGraphSizeY + GraphPosMin.Y
                    Next i
                Else
                    Dim iPt As Integer = 0
                    ptDraw = New PointF(iDCV / Me.Rough) {}
                    For i As Integer = 0 To iDCV Step Me.Rough
                        ptDraw(iPt).X = m_GraphDP(iLCnt)(i).X * sngGraphSizeX + GraphPosMin.X
                        ptDraw(iPt).Y = m_GraphDP(iLCnt)(i).Y * sngGraphSizeY + GraphPosMin.Y
                        iPt += 1
                    Next i
                End If
                Try
                    'データ内容を描画
                    Select Case Data(iLCnt).DrawType
                        Case 0
                            '線
                            If Data(iLCnt).Selected = False Then
                                grpGraph.DrawLines(Data(iLCnt).Pen, ptDraw)
                            Else
                                Dim penSel As Pen = New Pen(Data(iLCnt).Pen.Color, 5.0F)
                                grpGraph.DrawLines(penSel, ptDraw)
                            End If
                        Case 1
                            '点
                            Dim brs As Brush = New SolidBrush(Data(iLCnt).Pen.Color)
                            Dim iPCMax As Integer = Data(iLCnt).X.Count - 1
                            For iPCnt = 0 To iPCMax
                                '***************************************
                                'MOD START M_UNO 20160808  Ver.2.0.1.2  プロットの太さ指定
                                '   If Data(iLCnt).Bld(iPCnt) = True Then
                                '       grpGraph.FillEllipse(brs, CInt(ptDraw(iPCnt).X - 5.5F), CInt(ptDraw(iPCnt).Y - 5.5F), 11.0F, 11.0F)
                                '   Else
                                '       grpGraph.FillEllipse(brs, CInt(ptDraw(iPCnt).X - 3.5F), CInt(ptDraw(iPCnt).Y - 3.5F), 7.0F, 7.0F)
                                '   End If

                                Dim sPW As Single = Data(iLCnt).PltWidth
                                If Data(iLCnt).Bld(iPCnt) = True Then
                                    sPW = sPW + 3.0
                                End If
                                grpGraph.FillEllipse(brs, CInt(ptDraw(iPCnt).X - (sPW / 2)), CInt(ptDraw(iPCnt).Y - (sPW / 2)), sPW, sPW)
                                'MOD END M_UNO 20160808  Ver.2.0.1.2  プロットの太さ指定
                                '***************************************
                            Next iPCnt
                        Case 2
                            '両方
                            Dim brs As Brush = New SolidBrush(Data(iLCnt).Pen.Color)
                            For Each pt In ptDraw
                                grpGraph.FillEllipse(brs, pt.X - 3.0F, pt.Y - 3.0F, 7.0F, 7.0F)
                            Next
                            grpGraph.DrawLines(Data(iLCnt).Pen, ptDraw)
                    End Select
                Catch ex As OverflowException
                    'オーバーフロー時は描画しない。
                    Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
                End Try
            Next iLCnt

        End If
    End Sub

    '============================================
    'グリッド線の数値を描画
    '引数	grpGraph:		Graphicsオブジェクト
    '		sngGraphSizeX:	グラフ描画サイズ(X軸)
    '		sngGraphSizeY:	グラフ描画サイズ(Y軸)
    '		fontAxis:		数値描画フォント
    '戻り値	なし
    '============================================
    '''<summary>グリッド線の示す数値を描画する。</summary>(呼出元:sbRedrawGraph)
    Private Sub sbDrawGridValue(ByRef grpGraph As Graphics, ByVal sngGraphSizeX As Single, ByVal sngGraphSizeY As Single, ByRef fontAxis As Font)
        '目盛り毎の値の増分を計算
        Dim dDatStepX As Double = (XAxis.MaxVal - XAxis.MinVal) / 10.0R
        Dim dDatStepY1 As Double = (Y1Axis.MaxVal - Y1Axis.MinVal) / 10.0R
        Dim dDatStepY2 As Double = (Y2Axis.MaxVal - Y2Axis.MinVal) / 10.0R
        '目盛り毎の表示位置の増分を計算
        Dim dPosStepX As Double = (GraphPosMax.X - GraphPosMin.X) / 10.0R
        Dim dPosStepY As Double = (GraphPosMax.Y - GraphPosMin.Y) / 10.0R
        '軸の値を描画
        Dim strX As String
        Dim strY1 As String
        Dim strY2 As String
        For i As Integer = 0 To 10 Step 2
            Try
                If XAxis.Kind = 0 Then
                    strX = String.Format("{0:0.#}", XAxis.MinVal + dDatStepX * CDbl(i))
                Else
                    If XAxis.MaxVal - XAxis.MinVal > 20000000000000.0R Then
                        strX = String.Format("{0:yyyy/M/d}", DateTime.FromBinary(XAxis.MinVal + dDatStepX * CDbl(i)))
                    Else
                        strX = String.Format("{0:M/d HH:mm}", DateTime.FromBinary(XAxis.MinVal + dDatStepX * CDbl(i)))
                    End If
                    'strX = DateTime.FromFileTime(XAxis.MinVal + dDatStepX * CDbl(i)).ToString
                End If
                strY1 = String.Format("{0:0.#}", Y1Axis.MinVal + dDatStepY1 * CDbl(i))
                strY2 = String.Format("{0:0.#}", Y2Axis.MinVal + dDatStepY2 * CDbl(i))
                '***************************************
                'ADD START M_UNO 20170314 Ver.--- 相関グラフ
                'grpGraph.DrawString(strX, fontAxis, Brushes.Black,
                '                    GraphPosMin.X - 3.5F * strX.Length + CSng(dPosStepX) * CSng(i), GraphPosMax.Y + 7)
                'grpGraph.DrawString(strX, fontAxis, Brushes.Black,
                '                    GraphPosMin.X - 3.5F * strX.Length + CSng(dPosStepX) * CSng(i), GraphPosMax.Y + cX_SCALE_Y_DIFF)
                'ADD END M_UNO 20170314 Ver.--- 相関グラフ
                '***************************************
                grpGraph.DrawString(strY1, fontAxis, Brushes.Black,
                                    GraphPosMin.X - 7.0F * strY1.Length - 3.0F, GraphPosMax.Y - 6 - CSng(dPosStepY) * CSng(i))
                If Y2Axis.Enabled = True Then
                    grpGraph.DrawString(strY2, fontAxis, Brushes.Black,
                                        GraphPosMax.X + 2.0F, GraphPosMax.Y - 6 - CSng(dPosStepY) * CSng(i))
                End If
            Catch ex As OverflowException
                Debug.Print(System.Reflection.MethodBase.GetCurrentMethod.Name & " : " & ex.Message)
            End Try
        Next i
    End Sub

End Class
