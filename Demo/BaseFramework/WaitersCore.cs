using Demo.BaseFramework.LogTools;

namespace Demo.BaseFramework
{
    public class WaitersCore
    {
        /// <summary>
        /// Периодически выполняет код из <paramref name="conditionMethod"/> до тех пор пока он не вернёт true либо до истечения таймаута ожидания.
        /// </summary>
        /// <param name="conditionMethod"></param>
        /// <param name="retryInterval_s"></param>
        /// <param name="timeout_s"></param>
        /// <param name="waitDescription"></param>
        /// <returns>true если <paramref name="conditionMethod"/> вернул true</returns>
        public static bool WaitForConditionReached(
            Func<bool> conditionMethod,
            int retryInterval_s,
            int timeout_s,
            string waitDescription)
        {
            var limitTime = DateTime.Now.AddSeconds(timeout_s);
            Log.Info(waitDescription);

            while (true)
            {
                if (DateTime.Now <= limitTime)
                {
                    try
                    {
                        if (conditionMethod.Invoke())
                            return true;
                    }
                    catch (Exception) { }

                    Thread.Sleep(retryInterval_s * 1000);
                }
                else
                {
                    Log.Info("Достигнут таймаут");
                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// Приостанавливает выполнение на заданное количество секунд
        /// </summary>
        /// <param name="secondsAmount"></param>
        public static void Wait_s(int secondsAmount)
        {
            Thread.Sleep(secondsAmount * 1000);
        }
    }
}
