namespace PowerUp
{
    public class FrenzyPowerUp : PowerUp
    {
        private float duration;
        public override void OnCollect()
        {
            SetFrenzyAnimation();
            MakePlayerInvincible();
        }

        void SetFrenzyAnimation() { }

        void MakePlayerInvincible() { }

    }
}