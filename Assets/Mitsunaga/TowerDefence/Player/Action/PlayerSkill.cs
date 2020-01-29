using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerSkill : MonoBehaviour
{

    // スキルでの共通行動

    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    ParticleSystem skillParticle;

    void Start()
    {
        pManager.SkillTrigger
            .Subscribe(value =>
            {
                // スキル発動時の共通行動
                // 発動エフェクトの再生(なぜかエラーが出る)
                skillParticle.Play();
                Observable.Timer(System.TimeSpan.FromSeconds(skillParticle.main.duration))
                    .Subscribe(_ => skillParticle.Stop())
                    .AddTo(this.gameObject);

            }).AddTo(this.gameObject);
    }
}
