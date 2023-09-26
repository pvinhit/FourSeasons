import { React} from 'react';
import axios from 'axios';
import { Avatar, Upload, List, Tag, notification } from 'antd';
import ImgCrop from 'antd-img-crop';
import { UserOutlined } from '@ant-design/icons';
import '../../assests/css/userInfor.css'

//#region STYLE CONSTANTS
const colStyle = {
    backgroundColor: '#fff',
    padding: 20,
    minHeight: 300,
    borderRadius: 7,
    textAlign: 'center',
}
//#endregion

const UserAvatar = (props) => {
    //#region NOTIFICATION CONSTANT
    const [api, contextHolder] = notification.useNotification();
    const openNotification = (type, title, content, placement = 'topRight') => {
        api[type]({
            message: title,
            description: content,
            placement: placement
        });
    };
    //#endregion 

    //#region UPLOAD IMAGE
    const uploadButton = (
        <div>
            <Avatar
                icon={<UserOutlined />}
                size={{ xs: 50, sm: 70, md: 120, lg: 150, xl: 200, xxl: 250 }}
                style={{
                    backgroundColor: '#405a3d',
                }}
            />
        </div>
    );

    const handlePreview = async (file) => {
        let src = file.url;
        if (!src) {
            src = await new Promise((resolve) => {
                const reader = new FileReader();
                reader.readAsDataURL(file.originFileObj);
                reader.onload = () => resolve(reader.result);
            });
        }
        const image = new Image();
        image.src = src;
        const imgWindow = window.open(src);
        imgWindow?.document.write(image.outerHTML);
    };

    const handleChange = ({ fileList: newFileList }) => {
        props.setFileList(newFileList);
        handleUploadImage(newFileList);
        props.setDisableButtonSave(false);
    };

    const beforeUpload = (file) => {
        return false;
    };

    const handleUploadImage = (newFileList) => {
        const formData = new FormData();

        if (newFileList.length > 0) {
            formData.append("file", newFileList[0].originFileObj);
            formData.append("upload_preset", "ml_default");

            try {
                //Upload image to cloutdinary
                axios.post("https://api.cloudinary.com/v1_1/dfb1bpw8c/image/upload", formData)
                    .then((response) => {
                        var tempImage = [{
                            uid: response.data.public_id,
                            name: response.data.original_filename,
                            status: 'done',
                            url: response.data.url,
                        }];
                        props.setFileList([...tempImage]);

                        var tempData = [
                            { name: ['email'], value: props.userInfor[0].value },
                            { name: ['password'], value: props.userInfor[1].value },
                            { name: ['confirmPassword'], value: props.userInfor[2].value },
                            { name: ['fullName'], value: props.userInfor[3].value },
                            { name: ['address'], value: props.userInfor[4].value },
                            { name: ['phone'], value: props.userInfor[5].value },
                            { name: ['idcard'], value: props.userInfor[6].value },
                            { name: ['birthday'], value: props.userInfor[7].value },
                            { name: ['roleId'], value: props.userInfor[8].value },
                            { name: ['avatar'], value: response.data.url },
                            { name: ['bookingTimes'], value: props.userInfor[10].value },
                            { name: ['moneySpend'], value: props.userInfor[11].value },
                            { name: ['cancelBooking'], value: props.userInfor[12].value },
                        ];
                        props.setUserInfo([...tempData]);
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } catch (error) {
                console.log(error);
            }
        }
    };
    //#endregion

    return (
        <div style={colStyle}>
            <ImgCrop rotationSlider>
                <Upload
                    listType="picture-circle"
                    fileList={props.fileList}
                    onChange={handleChange}
                    onPreview={handlePreview}
                    beforeUpload={beforeUpload}
                    className='user-avatar-info'
                >
                    {props.fileList.length > 0 ? null : uploadButton}
                </Upload>
            </ImgCrop>
            {props.userInfor[8].value == 4 ? //display if current user role is Guest
                <List
                    itemLayout="horizontal"
                    style={{ textAlign: 'left' }}
                >
                    <List.Item>
                        <List.Item.Meta
                            title="Booking times"
                        />
                        <Tag color="magenta">
                            {props.userInfor[10].value}
                        </Tag>
                    </List.Item>
                    <List.Item>
                        <List.Item.Meta
                            title="Money Spent"
                        />
                        <Tag color="red">
                            {props.userInfor[11].value} $
                        </Tag>
                    </List.Item>
                    <List.Item>
                        <List.Item.Meta
                            title="Cancel Bookings"
                        />
                        <Tag color="default">
                            {props.userInfor[12].value}
                        </Tag>
                    </List.Item>
                </List> : null
            }
        </div>
    )
};

export default UserAvatar;