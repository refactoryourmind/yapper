﻿#pragma checksum "..\..\..\ChatWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "75448F2CC0B585D6CCEBA88A05543DA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Yapper.Client {
    
    
    /// <summary>
    /// ChatWindow
    /// </summary>
    public partial class ChatWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Yapper.Client.ChatWindow chatWindow;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox messageBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GridSplitter gridSplitter1;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox sendBox;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sendBtn;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBar statusBar1;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock statusBlk;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock pingBlk;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ipBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox portBox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button connectBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Yapper;component/chatwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ChatWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.chatWindow = ((Yapper.Client.ChatWindow)(target));
            
            #line 4 "..\..\..\ChatWindow.xaml"
            this.chatWindow.Loaded += new System.Windows.RoutedEventHandler(this.chatWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.messageBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.gridSplitter1 = ((System.Windows.Controls.GridSplitter)(target));
            return;
            case 4:
            this.sendBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 25 "..\..\..\ChatWindow.xaml"
            this.sendBox.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.sendBox_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.sendBtn = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\ChatWindow.xaml"
            this.sendBtn.Click += new System.Windows.RoutedEventHandler(this.sendBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.statusBar1 = ((System.Windows.Controls.Primitives.StatusBar)(target));
            return;
            case 7:
            this.statusBlk = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.pingBlk = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.ipBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.portBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.connectBtn = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\ChatWindow.xaml"
            this.connectBtn.Click += new System.Windows.RoutedEventHandler(this.connectBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

