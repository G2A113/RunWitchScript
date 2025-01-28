using UnityEngine;

/// <summary>
/// �J�����̓����ƃv���C���[�̃X�R�A�ɉ��������x�Ǘ����s���N���X
/// </summary>
public class Camera : MonoBehaviour {

    [Header("�J�����̓����ݒ�")]
    public Rigidbody2D _rigidbody; // �J�����̃��W�b�h�{�f�B�i�����̐���Ɏg�p�j
    [SerializeField] private float _speed = 6; // ���x�i�������ɃJ�����𓮂������x�j

    /// <summary>
    /// ����������
    /// �J�����̃��W�b�h�{�f�B���擾
    /// </summary>
    void Start() {
        // �J�����̃��W�b�h�{�f�B���擾���A�ϐ��Ɋi�[
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ���t���[���̍X�V����
    /// �J���������̑��x�ňړ�������
    /// </summary>
    void Update() {
        // �������iX���j�̑��x��ݒ肵�A���������iY���j�͂��̂܂܂ɂ���
        _rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
    }
}
