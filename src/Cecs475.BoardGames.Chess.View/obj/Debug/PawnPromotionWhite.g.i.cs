﻿#pragma checksum "..\..\PawnPromotionWhite.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F5FC8F18985170E363A0C4ABF927018"
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
    /// PawnPromotionWindow
    /// </summary>
    public partial class PawnPromotionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\PawnPromotionWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button QueenPromote;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\PawnPromotionWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RookPromote;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\PawnPromotionWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BishopPromote;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\PawnPromotionWhite.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button KnightPromote;
        
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
            System.Uri resourceLocater = new System.Uri("/Cecs475.BoardGames.Chess.View;component/pawnpromotionwhite.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PawnPromotionWhite.xaml"
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
            this.QueenPromote = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\PawnPromotionWhite.xaml"
            this.QueenPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RookPromote = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\PawnPromotionWhite.xaml"
            this.RookPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BishopPromote = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\PawnPromotionWhite.xaml"
            this.BishopPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            case 4:
            this.KnightPromote = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\PawnPromotionWhite.xaml"
            this.KnightPromote.Click += new System.Windows.RoutedEventHandler(this.promoteChoice);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

