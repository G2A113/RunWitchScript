using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �v���C���[�����˂���e�ۂ̃I�u�W�F�N�g�v�[��
/// </summary>
public class ObjectPoolController : MonoBehaviour {
    // �e�̃v���t�@�u
    [SerializeField] private BulletControl _bullet;
    // ��������e�̍ő吔
    [SerializeField] private int _maxCount;
    // ���������e���i�[����Queue�i������o���j
    [SerializeField] private Queue<BulletControl> _bulletQueue;
    // ���񐶐����̃|�W�V����
    private Vector3 _setPos = new Vector3(100, 100, 0);

    /// <summary>
    /// �N�����ɃI�u�W�F�N�g�v�[�������������A�e������̐�������������Queue�Ɋi�[����B
    /// </summary>
    private void Awake() {
        // Queue�̏�����
        _bulletQueue = new Queue<BulletControl>();

        // �w�肳�ꂽ�������e�𐶐����AQueue�ɒǉ�
        for (int i = 0; i < _maxCount; i++) {
            // �e�𐶐�
            BulletControl tmpBullet = Instantiate(_bullet, _setPos, Quaternion.identity, transform);
            // Queue�ɒe��ǉ�
            _bulletQueue.Enqueue(tmpBullet);
        }
    }

    /// <summary>
    /// �e���I�u�W�F�N�g�v�[������݂��o���B
    /// </summary>
    /// <param name="pos">�e���o��������ʒu</param>
    /// <returns>�݂��o���ꂽ�e��BulletControl�I�u�W�F�N�g</returns>
    public BulletControl Launch(Vector3 pos) {
        // Queue����̏ꍇ�A�e��݂��o���Ȃ��̂�null��Ԃ�
        if (_bulletQueue.Count <= 0) {
            return null; // �e���Ȃ��ꍇ��null��Ԃ�
        }

        // Queue����e�����o��
        BulletControl tmpBullet = _bulletQueue.Dequeue();
        // �e��\��
        tmpBullet.gameObject.SetActive(true);
        // �e���w�肳�ꂽ�ʒu�Ɉړ�
        tmpBullet.ShowInStage(pos);
        // �Ăяo�����ɒe��Ԃ�
        return tmpBullet;
    }

    /// <summary>
    /// �g�p�����e���I�u�W�F�N�g�v�[���ɉ������B
    /// </summary>
    /// <param name="bullet">�������e��BulletControl�I�u�W�F�N�g</param>
    public void Collect(BulletControl bullet) {
        // �e�̃Q�[���I�u�W�F�N�g���\���ɂ���
        bullet.gameObject.SetActive(false);
        // �e��Queue�ɖ߂�
        _bulletQueue.Enqueue(bullet);
    }
}
