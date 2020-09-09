using System;

namespace FirClient.Logic.Event
{
    public interface ISceneEvent
    {
        void OnExecute(string param, Action moveNext);
    }
}