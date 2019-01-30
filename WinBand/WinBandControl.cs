﻿///------------------------------------------------------------------------------
/// @ Y_Theta
///------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsDeskBand.DeskBand;
using WindowsDeskBand.DeskBand.BandParts;
using WindowsDeskBand.DeskBand.Interop.COM;
using WindowsDeskBand.DeskBand.Interop.Struct;

namespace WinBand {
    /// <summary>
    /// Winforms implementation of <see cref="IDeskBandCore"/>. The deskband should inherit this class.
    /// The deskband should also have these attributes <see cref="ComVisibleAttribute"/>, <see cref="GuidAttribute"/>, <see cref="BandRegistrationAttribute"/>.
    /// </summary>
    public class WinBandControl : UserControl, IDeskBandCore {
        /// <summary>
        /// Options for this deskband.
        /// </summary>
        /// <seealso cref="CSDeskBandOptions"/>
        protected BandOptions Options { get; } = new BandOptions();

        /// <summary>
        /// Get the current taskbar information.
        /// </summary>
        /// <seealso cref="TaskbarInfo"/>
        protected TaskbarInfo TaskbarInfo { get; }

        private readonly BandCore _impl;
        private readonly Guid _deskbandGuid;

        /// <summary>
        /// Initializes a new instance of <see cref="WinBandControl"/>.
        /// </summary>
        public WinBandControl() {
            try {
                Options.Title = BandCore.GetToolbarName(GetType());

                _impl = new BandCore(Handle, Options);
                _impl.VisibilityChanged += VisibilityChanged;
                _impl.Closed += OnClose;

                TaskbarInfo = _impl.TaskbarInfo;
                SizeChanged += CSDeskBandWin_SizeChanged;

                //Empty guid is a workaround for winforms designer because there will be no guid attribute
                _deskbandGuid = new Guid(GetType().GetCustomAttribute<GuidAttribute>(true)?.Value ?? Guid.Empty.ToString("B"));
            }
            catch {
                throw;
            }
        }

        private void CSDeskBandWin_SizeChanged(object sender, EventArgs e) {
            if (TaskbarInfo.Orientation == TaskbarOrientation.Horizontal) {
                Options.HorizontalSize = Size;
            }
            else {
                Options.VerticalSize = Size;
            }
        }

        private void OnClose(object sender, EventArgs eventArgs) {
            OnClose();
        }

        /// <summary>
        /// Method is called when deskband is being closed.
        /// </summary>
        protected virtual void OnClose() { }

        private void VisibilityChanged(object sender, VisibilityChangedEventArgs visibilityChangedEventArgs) {
            VisibilityChanged(visibilityChangedEventArgs.IsVisible);
        }

        protected virtual void VisibilityChanged(bool visible) {
            if (visible) {
                Show();
            }
            else {
                Hide();
            }
        }


        /// <summary>
        /// Close the deskband.
        /// </summary>
        protected void CloseDeskBand() {
            _impl.CloseDeskBand();
        }

        [ComRegisterFunction]
        private static void Register(Type t) {
            BandCore.Register(t);
        }

        [ComUnregisterFunction]
        private static void Unregister(Type t) {
            BandCore.Unregister(t);
        }

        int IDeskBand2.ShowDW(bool fShow) {
            return _impl.ShowDW(fShow);
        }

        int IDeskBand2.CloseDW(uint dwReserved) {
            return _impl.CloseDW(dwReserved);
        }

        int IDeskBand2.ResizeBorderDW(RECT prcBorder, IntPtr punkToolbarSite, bool fReserved) {
            return _impl.ResizeBorderDW(prcBorder, punkToolbarSite, fReserved);
        }

        int IDeskBand2.GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi) {
            return _impl.GetBandInfo(dwBandID, dwViewMode, ref pdbi);
        }

        int IDeskBand2.CanRenderComposited(out bool pfCanRenderComposited) {
            return _impl.CanRenderComposited(out pfCanRenderComposited);
        }

        int IDeskBand2.SetCompositionState(bool fCompositionEnabled) {
            return _impl.SetCompositionState(fCompositionEnabled);
        }

        int IDeskBand2.GetCompositionState(out bool pfCompositionEnabled) {
            return _impl.GetCompositionState(out pfCompositionEnabled);
        }

        int IDeskBand2.GetWindow(out IntPtr phwnd) {
            return _impl.GetWindow(out phwnd);
        }

        int IDeskBand2.ContextSensitiveHelp(bool fEnterMode) {
            return _impl.ContextSensitiveHelp(fEnterMode);
        }

        int IDeskBand.GetWindow(out IntPtr phwnd) {
            return _impl.GetWindow(out phwnd);
        }

        int IDeskBand.ContextSensitiveHelp(bool fEnterMode) {
            return _impl.ContextSensitiveHelp(fEnterMode);
        }

        int IDeskBand.ShowDW(bool fShow) {
            return _impl.ShowDW(fShow);
        }

        int IDeskBand.CloseDW(uint dwReserved) {
            return _impl.CloseDW(dwReserved);
        }

        int IDeskBand.ResizeBorderDW(RECT prcBorder, IntPtr punkToolbarSite, bool fReserved) {
            return _impl.ResizeBorderDW(prcBorder, punkToolbarSite, fReserved);
        }

        int IDeskBand.GetBandInfo(uint dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi) {
            return _impl.GetBandInfo(dwBandID, dwViewMode, ref pdbi);
        }

        int IDockingWindow.ShowDW(bool fShow) {
            return _impl.ShowDW(fShow);
        }

        int IDockingWindow.CloseDW(uint dwReserved) {
            return _impl.CloseDW(dwReserved);
        }

        int IDockingWindow.ResizeBorderDW(RECT prcBorder, IntPtr punkToolbarSite, bool fReserved) {
            return _impl.ResizeBorderDW(prcBorder, punkToolbarSite, fReserved);
        }

        int IOleWindow.GetWindow(out IntPtr phwnd) {
            return _impl.GetWindow(out phwnd);
        }

        int IOleWindow.ContextSensitiveHelp(bool fEnterMode) {
            return _impl.ContextSensitiveHelp(fEnterMode);
        }

        int IObjectWithSite.SetSite(object pUnkSite) {
            return _impl.SetSite(pUnkSite);
        }

        int IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite) {
            return _impl.GetSite(ref riid, out ppvSite);
        }

        int IContextMenu3.QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags) {
            return _impl.QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
        }

        int IContextMenu3.InvokeCommand(IntPtr pici) {
            return _impl.InvokeCommand(pici);
        }

        int IContextMenu3.GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, out string pcszName, uint cchMax) {
            return _impl.GetCommandString(ref idcmd, uflags, ref pwReserved, out pcszName, cchMax);
        }

        int IContextMenu3.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam) {
            return _impl.HandleMenuMsg(uMsg, wParam, lParam);
        }

        int IContextMenu3.HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr plResult) {
            return _impl.HandleMenuMsg2(uMsg, wParam, lParam, out plResult);
        }

        int IContextMenu2.QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags) {
            return _impl.QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
        }

        int IContextMenu2.InvokeCommand(IntPtr pici) {
            return _impl.InvokeCommand(pici);
        }

        int IContextMenu2.GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, out string pcszName, uint cchMax) {
            return _impl.GetCommandString(ref idcmd, uflags, ref pwReserved, out pcszName, cchMax);
        }

        int IContextMenu2.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam) {
            return _impl.HandleMenuMsg(uMsg, wParam, lParam);
        }

        int IContextMenu.QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, QueryContextMenuFlags uFlags) {
            return _impl.QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
        }

        int IContextMenu.InvokeCommand(IntPtr pici) {
            return _impl.InvokeCommand(pici);
        }

        int IContextMenu.GetCommandString(ref uint idcmd, uint uflags, ref uint pwReserved, out string pcszName, uint cchMax) {
            return _impl.GetCommandString(ref idcmd, uflags, ref pwReserved, out pcszName, cchMax);
        }

        int IPersistStream.GetClassID(out Guid pClassID) {
            pClassID = _deskbandGuid;
            return HRESULT.S_OK;
        }

        int IPersistStream.GetSizeMax(out ulong pcbSize) {
            return _impl.GetSizeMax(out pcbSize);
        }

        int IPersistStream.IsDirty() {
            return _impl.IsDirty();
        }

        int IPersistStream.Load(object pStm) {
            return _impl.Load(pStm);
        }

        int IPersistStream.Save(IntPtr pStm, bool fClearDirty) {
            return _impl.Save(pStm, fClearDirty);
        }

        int IPersist.GetClassID(out Guid pClassID) {
            pClassID = _deskbandGuid;
            return HRESULT.S_OK;
        }

        int IInputObject.UIActivateIO(bool fActivate, ref MSG msg) {
            return _impl.UIActivateIO(fActivate, ref msg);
        }

        int IInputObject.HasFocusIO() {
            return _impl.HasFocusIO();
        }

        int IInputObject.TranslateAcceleratorIO(ref MSG msg) {
            return _impl.TranslateAcceleratorIO(ref msg);
        }
    }
}
