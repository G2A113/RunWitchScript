using UnityEngine;
/// <summary>
/// SE�̊i�[
/// </summary>
public class SEScript : MonoBehaviour {
    // AudioSource�R���|�[�l���g�̎Q��
    private AudioSource _audioSourceSE;

    // SE (���ʉ�) �p��AudioClip�z��
    [Header("SE")]
    [SerializeField] private AudioClip[] _audioClip = new AudioClip[5];

    // SE���Đ����邽�߂̃f���Q�[�g�^se�̒�`
    public delegate void se();

    // �󒆍U���̌��ʉ����i�[����z��
    public se[] _airAttackSE = new se[3];

    // ���������̏���
    void Start() {
        // AudioSource�R���|�[�l���g���擾
        _audioSourceSE = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �W�����v���̌��ʉ����Đ����鏈��
    /// </summary>
    public void JumpSE() {
        _audioSourceSE.PlayOneShot(_audioClip[0]);
    }

    /// <summary>
    /// �e�̔��ˎ��̌��ʉ����Đ����鏈��
    /// </summary>
    public void BulletSE() {
        _audioSourceSE.PlayOneShot(_audioClip[1]);
    }

    /// <summary>
    /// ���U�������̌��ʉ����Đ����鏈��
    /// </summary>
    public void SwingSE() {
        _audioSourceSE.PlayOneShot(_audioClip[2]);
    }

    /// <summary>
    /// ���S���̌��ʉ����Đ����鏈��
    /// </summary>
    public void DeadSE() {
        _audioSourceSE.PlayOneShot(_audioClip[3]);
    }

    /// <summary>
    /// 㩎��̌��ʉ����Đ����鏈��
    /// </summary>
    public void PoisonSE() {
        _audioSourceSE.PlayOneShot(_audioClip[4]);
    }

    /// <summary>
    /// �������̌��ʉ����Đ����鏈��
    /// </summary>
    public void BombSE() {
        _audioSourceSE.PlayOneShot(_audioClip[5]);
    }

    /// <summary>
    /// �ǔj�󎞂̌��ʉ����Đ����鏈��
    /// </summary>
    public void WallBreakSE() {
        _audioSourceSE.PlayOneShot(_audioClip[6]);
    }

    /// <summary>
    /// �V�[���h�j�󎞂̌��ʉ����Đ����鏈��
    /// </summary>
    public void ShieldBreakSE() {
        _audioSourceSE.PlayOneShot(_audioClip[7]);
    }

    /// <summary>
    /// �G�l�~�[�@�A�C�U�����̌��ʉ����Đ����鏈��
    /// </summary>
    public void SunderSE() {
        _audioSourceSE.PlayOneShot(_audioClip[8]);
    }

    /// <summary>
    /// �G�l�~�[�@�S�[�X�g�U�����̌��ʉ����Đ����鏈��
    /// </summary>
    public void GhostSE() {
        _audioSourceSE.PlayOneShot(_audioClip[9]);
    }

    /// <summary>
    /// �v���C���[��e�����̌��ʉ����Đ����鏈��
    /// </summary>
    public void ImpactSE() {
        _audioSourceSE.PlayOneShot(_audioClip[10]);
    }

    /// <summary>
    /// �G�l�~�[�̍U���������̌��ʉ����Đ����鏈��
    /// </summary>
    public void CounterSE() {
        _audioSourceSE.PlayOneShot(_audioClip[11]);
    }
}
