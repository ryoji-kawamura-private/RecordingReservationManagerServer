<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formSettingChannel
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(formSettingChannel))
        Me.btnDeleteChannel = New System.Windows.Forms.Button
        Me.btnAllCancel = New System.Windows.Forms.Button
        Me.lblTitleTitle = New System.Windows.Forms.Label
        Me.lblStationTitle = New System.Windows.Forms.Label
        Me.dgvStation = New System.Windows.Forms.DataGridView
        Me.stationMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.editToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.deleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnReflectChannel = New System.Windows.Forms.Button
        Me.txtStationName = New RK.BaseTextBox
        Me.txtChannel = New RK.NumericTextBox
        Me.btnEditChannel = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnImport = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtServiceID = New RK.NumericTextBox
        CType(Me.dgvStation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.stationMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnDeleteChannel
        '
        Me.btnDeleteChannel.Location = New System.Drawing.Point(6, 106)
        Me.btnDeleteChannel.Name = "btnDeleteChannel"
        Me.btnDeleteChannel.Size = New System.Drawing.Size(94, 23)
        Me.btnDeleteChannel.TabIndex = 8
        Me.btnDeleteChannel.Text = "削除"
        '
        'btnAllCancel
        '
        Me.btnAllCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAllCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAllCancel.Location = New System.Drawing.Point(198, 318)
        Me.btnAllCancel.Name = "btnAllCancel"
        Me.btnAllCancel.Size = New System.Drawing.Size(94, 23)
        Me.btnAllCancel.TabIndex = 13
        Me.btnAllCancel.Text = "キャンセル"
        '
        'lblTitleTitle
        '
        Me.lblTitleTitle.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitleTitle.Location = New System.Drawing.Point(4, 14)
        Me.lblTitleTitle.Name = "lblTitleTitle"
        Me.lblTitleTitle.Size = New System.Drawing.Size(94, 12)
        Me.lblTitleTitle.TabIndex = 0
        Me.lblTitleTitle.Text = "放送局："
        Me.lblTitleTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblStationTitle
        '
        Me.lblStationTitle.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStationTitle.Location = New System.Drawing.Point(4, 64)
        Me.lblStationTitle.Name = "lblStationTitle"
        Me.lblStationTitle.Size = New System.Drawing.Size(94, 12)
        Me.lblStationTitle.TabIndex = 6
        Me.lblStationTitle.Text = "登録済放送局："
        Me.lblStationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvStation
        '
        Me.dgvStation.AllowUserToAddRows = False
        Me.dgvStation.AllowUserToDeleteRows = False
        Me.dgvStation.AllowUserToResizeColumns = False
        Me.dgvStation.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.dgvStation.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvStation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvStation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvStation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvStation.ContextMenuStrip = Me.stationMenuStrip
        Me.dgvStation.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvStation.Location = New System.Drawing.Point(102, 62)
        Me.dgvStation.MultiSelect = False
        Me.dgvStation.Name = "dgvStation"
        Me.dgvStation.ReadOnly = True
        Me.dgvStation.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvStation.RowTemplate.Height = 21
        Me.dgvStation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvStation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvStation.Size = New System.Drawing.Size(285, 250)
        Me.dgvStation.StandardTab = True
        Me.dgvStation.TabIndex = 11
        '
        'stationMenuStrip
        '
        Me.stationMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.editToolStripMenuItem, Me.deleteToolStripMenuItem})
        Me.stationMenuStrip.Name = "DeleteMenuStrip"
        Me.stationMenuStrip.Size = New System.Drawing.Size(137, 48)
        '
        'editToolStripMenuItem
        '
        Me.editToolStripMenuItem.Name = "editToolStripMenuItem"
        Me.editToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.editToolStripMenuItem.Text = "選択行編集"
        '
        'deleteToolStripMenuItem
        '
        Me.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem"
        Me.deleteToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.deleteToolStripMenuItem.Text = "削除"
        '
        'btnReflectChannel
        '
        Me.btnReflectChannel.Location = New System.Drawing.Point(100, 36)
        Me.btnReflectChannel.Name = "btnReflectChannel"
        Me.btnReflectChannel.Size = New System.Drawing.Size(220, 23)
        Me.btnReflectChannel.TabIndex = 4
        Me.btnReflectChannel.Text = "▼　追加　▼"
        Me.btnReflectChannel.UseVisualStyleBackColor = True
        '
        'txtStationName
        '
        Me.txtStationName.ErrorCode = ""
        Me.txtStationName.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStationName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtStationName.Location = New System.Drawing.Point(254, 12)
        Me.txtStationName.MaxLengthByte = CType(20, Short)
        Me.txtStationName.Name = "txtStationName"
        Me.txtStationName.SearchBackColor = System.Drawing.Color.LightPink
        Me.txtStationName.SelectionBackColor = System.Drawing.Color.LightSteelBlue
        Me.txtStationName.Size = New System.Drawing.Size(134, 19)
        Me.txtStationName.TabIndex = 3
        '
        'txtChannel
        '
        Me.txtChannel.AutoFormat = False
        Me.txtChannel.AutoResize = False
        Me.txtChannel.Delimiter = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtChannel.ErrorCode = ""
        Me.txtChannel.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtChannel.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtChannel.InputAlpha = False
        Me.txtChannel.InputEtc = False
        Me.txtChannel.InputKana = False
        Me.txtChannel.InputZenkaku = False
        Me.txtChannel.Location = New System.Drawing.Point(100, 12)
        Me.txtChannel.MaxLengthByte = CType(0, Short)
        Me.txtChannel.MaxValue = CType(9223372036854775807, Long)
        Me.txtChannel.MinValue = CType(0, Long)
        Me.txtChannel.Name = "txtChannel"
        Me.txtChannel.NumericAccuracy = New RK.Accuracy(CType(20, Short), CType(0, Short))
        Me.txtChannel.SearchBackColor = System.Drawing.Color.LightPink
        Me.txtChannel.SelectionBackColor = System.Drawing.Color.LightSteelBlue
        Me.txtChannel.Size = New System.Drawing.Size(122, 19)
        Me.txtChannel.TabIndex = 1
        Me.txtChannel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtChannel.WordWrap = False
        '
        'btnEditChannel
        '
        Me.btnEditChannel.Location = New System.Drawing.Point(6, 80)
        Me.btnEditChannel.Name = "btnEditChannel"
        Me.btnEditChannel.Size = New System.Drawing.Size(94, 23)
        Me.btnEditChannel.TabIndex = 7
        Me.btnEditChannel.Text = "選択行編集"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(322, 36)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(66, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(6, 132)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(94, 23)
        Me.btnExport.TabIndex = 9
        Me.btnExport.Text = "エクスポート"
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(6, 158)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(94, 23)
        Me.btnImport.TabIndex = 10
        Me.btnImport.Text = "インポート"
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Location = New System.Drawing.Point(294, 318)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(94, 23)
        Me.btnApply.TabIndex = 14
        Me.btnApply.Text = "適用"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(102, 318)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(94, 23)
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "OK"
        '
        'txtServiceID
        '
        Me.txtServiceID.AutoFormat = False
        Me.txtServiceID.AutoResize = False
        Me.txtServiceID.Delimiter = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtServiceID.ErrorCode = ""
        Me.txtServiceID.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtServiceID.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtServiceID.InputAlpha = False
        Me.txtServiceID.InputEtc = False
        Me.txtServiceID.InputKana = False
        Me.txtServiceID.InputZenkaku = False
        Me.txtServiceID.Location = New System.Drawing.Point(224, 12)
        Me.txtServiceID.MaxLengthByte = CType(0, Short)
        Me.txtServiceID.MaxValue = CType(999, Long)
        Me.txtServiceID.MinValue = CType(0, Long)
        Me.txtServiceID.Name = "txtServiceID"
        Me.txtServiceID.NumericAccuracy = New RK.Accuracy(CType(2, Short), CType(0, Short))
        Me.txtServiceID.SearchBackColor = System.Drawing.Color.LightPink
        Me.txtServiceID.SelectionBackColor = System.Drawing.Color.LightSteelBlue
        Me.txtServiceID.Size = New System.Drawing.Size(28, 19)
        Me.txtServiceID.TabIndex = 2
        Me.txtServiceID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtServiceID.WordWrap = False
        '
        'formSettingChannel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 346)
        Me.Controls.Add(Me.txtServiceID)
        Me.Controls.Add(Me.txtStationName)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnEditChannel)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnReflectChannel)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtChannel)
        Me.Controls.Add(Me.lblStationTitle)
        Me.Controls.Add(Me.dgvStation)
        Me.Controls.Add(Me.lblTitleTitle)
        Me.Controls.Add(Me.btnDeleteChannel)
        Me.Controls.Add(Me.btnAllCancel)
        Me.Controls.Add(Me.btnApply)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "formSettingChannel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "放送局登録"
        CType(Me.dgvStation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.stationMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnDeleteChannel As System.Windows.Forms.Button
    Friend WithEvents btnAllCancel As System.Windows.Forms.Button
    Friend WithEvents lblTitleTitle As System.Windows.Forms.Label
    Friend WithEvents lblStationTitle As System.Windows.Forms.Label
    Friend WithEvents dgvStation As System.Windows.Forms.DataGridView
    Friend WithEvents txtChannel As RK.NumericTextBox
    Friend WithEvents btnReflectChannel As System.Windows.Forms.Button
    Friend WithEvents txtStationName As RK.BaseTextBox
    Friend WithEvents stationMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents deleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnEditChannel As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents editToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtServiceID As RK.NumericTextBox

End Class
