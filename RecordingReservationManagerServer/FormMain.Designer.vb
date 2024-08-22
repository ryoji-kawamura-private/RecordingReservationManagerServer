Imports System.Security.Principal

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.ReservationIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ReservationMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FinishToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReservationMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReservationIcon
        '
        Me.ReservationIcon.BalloonTipText = "Record"
        Me.ReservationIcon.ContextMenuStrip = Me.ReservationMenuStrip
        Me.ReservationIcon.Icon = CType(resources.GetObject("ReservationIcon.Icon"), System.Drawing.Icon)
        Me.ReservationIcon.Text = "録画予約マネージャ"
        Me.ReservationIcon.Visible = True
        '
        'ReservationMenuStrip
        '
        Me.ReservationMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingToolStripMenuItem, Me.FinishToolStripMenuItem})
        Me.ReservationMenuStrip.Name = "ContextMenuStrip1"
        Me.ReservationMenuStrip.Size = New System.Drawing.Size(132, 48)
        '
        'SettingToolStripMenuItem
        '
        Me.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem"
        Me.SettingToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.SettingToolStripMenuItem.Text = "オプション"
        '
        'FinishToolStripMenuItem
        '
        Me.FinishToolStripMenuItem.Name = "FinishToolStripMenuItem"
        Me.FinishToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.FinishToolStripMenuItem.Text = "終了"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(23, 8)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormMain"
        Me.ShowInTaskbar = False
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.ReservationMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ReservationIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents ReservationMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FinishToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
