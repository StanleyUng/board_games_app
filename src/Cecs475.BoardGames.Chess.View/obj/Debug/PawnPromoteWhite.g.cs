﻿#pragma checksum "..\..\PawnPromoteWhite.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "19B7ECDAA594A11925EF15879EFB3BEF5A8387D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Cecs475.BoardGames.Chess.View;
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


namespace Cecs475.BoardGames.Chess.View {
    
    
    /// <summary>
    /// PawnPromoteWhite
    /// </summary>
    public partial class PawnPromoteWhite : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\PawnPromoteWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WhiteQueenPromote;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\PawnPromoteWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WhiteRookPromote;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\PawnPromoteWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WhiteBishopPromote;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\PawnPromoteWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WhiteKnightPromote;
        
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
            System.Uri resourceLocater = new System.Uri("/Cecs475.BoardGames.Chess.View;component/pawnpromotewhite.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PawnPromoteWhite.xaml"
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
            this.WhiteQueenPromote = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\PawnPromoteWhite.xaml"
            this.WhiteQueenPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 2:
            this.WhiteRookPromote = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\PawnPromoteWhite.xaml"
            this.WhiteRookPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 3:
            this.WhiteBishopPromote = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\PawnPromoteWhite.xaml"
            this.WhiteBishopPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 4:
            this.WhiteKnightPromote = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\PawnPromoteWhite.xaml"
            this.WhiteKnightPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
