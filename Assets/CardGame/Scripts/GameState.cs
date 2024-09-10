namespace Com.SmokeyShadow.Trumpolia
{
    public class GameState
    {
        #region STATIC FIELDS
        private static GameState _instance;
        #endregion

        #region FIELDS
        private State currentState;
        #endregion

        #region PROPERTIES
        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameState();
                return _instance;
            }
        }
        #endregion

        #region ENUMS
        public enum State { BoardGenerate, Play, LockBoard, Pause, Finished }
        #endregion

        #region PUBLIC METHODS
        public State GetState()
        {
            return currentState;
        }

        public void ChangeState(State state)
        {
            currentState = state;
        }
        #endregion
    }
}