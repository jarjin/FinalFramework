using LuaInterface;
using SuperScrollView;

namespace FirClient.Extensions
{
    public static class LoopListView2Extensions
    {
        public static void InitListView(this LoopListView2 view, LuaTable self, int num, LuaFunction onUpdate)
        {
            if (view != null)
            {
                view.InitListView(num, (LoopListView2 arg1, int arg2) => onGetItemByIndex(arg1, arg2, self, onUpdate));
            }
        }

        static LoopListViewItem2 onGetItemByIndex(LoopListView2 arg1, int arg2, LuaTable self, LuaFunction func)
        {
            return func.Invoke<LuaTable, int, LoopListViewItem2>(self, arg2);
        }

        public static void DestroyListView(this LoopListView2 view, LuaTable self, LuaFunction onUpdate)
        {
            if (self != null) { self.Dispose(); }
            if (onUpdate != null) { onUpdate.Dispose(); }
        }
    }
}

