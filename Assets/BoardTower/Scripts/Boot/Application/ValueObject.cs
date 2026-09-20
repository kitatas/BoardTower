using BoardTower.Common.Application;
using UnityEngine;

namespace BoardTower.Boot.Application
{
    public sealed class BootModalVO : BaseModalVO<BootModalType>
    {
        public BootModalVO(BootModalType type, Fade fade) : base(type, fade)
        {
            if (type is BootModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_BOOT_MODAL);
        }
    }

    public sealed class BootModalTransitionVO : BaseModalTransitionVO<BootModalType>
    {
        public BootModalTransitionVO(BootModalType type, TransitionVO transition) : base(type, transition)
        {
            if (type is BootModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_BOOT_MODAL);
        }

        public static BootModalTransitionVO Create(BootModalType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new BootModalTransitionVO(type, transition);
        }

        public static BootModalTransitionVO Create(BootModalVO bootModal, float duration)
        {
            var transition = new TransitionVO(bootModal.fade, duration);
            return new BootModalTransitionVO(bootModal.type, transition);
        }
    }

    public sealed class DisplayNameTransitionVO
    {
        public readonly TransitionVO transition;

        public DisplayNameTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static DisplayNameTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new DisplayNameTransitionVO(transition);
        }
    }

    public sealed class SplashVO
    {
        public readonly SplashType type;
        public readonly Sprite sprite;

        public SplashVO(SplashType type, Sprite sprite)
        {
            if (type is SplashType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SPLASH);

            this.type = type;
            this.sprite = sprite;
        }
    }

    public sealed class SplashTransitionVO
    {
        public readonly SplashVO splash;
        public readonly TransitionVO transition;

        public SplashTransitionVO(SplashVO splash, TransitionVO transition)
        {
            this.splash = splash;
            this.transition = transition;
        }

        public static SplashTransitionVO Create(SplashVO splash, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new SplashTransitionVO(splash, transition);
        }
    }

    public sealed class UpdateTransitionVO
    {
        public readonly TransitionVO transition;

        public UpdateTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static UpdateTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new UpdateTransitionVO(transition);
        }
    }

    public sealed class AppVersionVO
    {
        public readonly int major;
        public readonly int minor;

        public AppVersionVO(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
        }
    }
}