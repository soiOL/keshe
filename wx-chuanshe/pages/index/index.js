//index.js
//获取应用实例
const app = getApp()

Page({
  data: {
    socketMsg: "嘻嘻",
    intensity: 0,
    tem: 0,
    humidity: 0,
    swMap: { "Fan": false, "Led1": false, "Led2": false, "Led3": false },
    sensorStatus: { "Fan": false, "Is": false, "Tah": false },
    deviceList: [],
    deviceStatus: {},
    devicePos: 0
  },
  deviceStatus: {},
  deviceDetail: new Object(),
  liveData: { "intensity": [0], "temperature": [0], "humidity": [0], "time": ["00:00:00"], "deviceId": "" },
  deviceId: "null",
  isSocketOpen: false,
  swPosMap: { "swFan": 0, "swLed1": 1, "swLed2": 2, "swLed3": 3 },
  wxcode: "",
  openChart: function (e) {
    var that = this
    wx.navigateTo({
      url: '../chart/chart?deviceId=' + that.data.deviceList[that.data.devicePos].deviceId + "&id=" + that.data.deviceList[that.data.devicePos].id
    })
  },
  deleteDevice: function (e) {
    console.log(e)
    var id = e.currentTarget.id
    var that = this
    wx.showModal({
      title: '警告',
      content: '删除设备' + that.data.deviceList[id]["id"] + "(设备号：" + that.data.deviceList[id]["deviceId"] + ")",
      success: function (res) {
        if (res.confirm) {
          var msg = new Object()
          msg["action"] = "unbind"
          msg["deviceId"] = that.data.deviceList[id]["deviceId"]

          if (that.isSocketOpen) {
            console.log(msg)
            that.socketSendMsg(msg)
          }
        }
      }
    })
  },
  selectDevice: function (event) {
    console.log(event)
    var id = this.data.deviceList[event.currentTarget.id].deviceId
    console.log(id)
    if(this.deviceDetail[id] === undefined){
      this.deviceDetail[id] = new Object()
      this.deviceDetail[id].swMap = { "Fan": false, "Led1": false, "Led2": false, "Led3": false }
      this.deviceDetail[id].intensity = 0

      this.deviceDetail[id].tem = 0
      this.deviceDetail[id].humidity = 0
      this.deviceDetail[id].sensorStatus = { "Fan": false, "Is": false, "Tah": false }
      console.log("没有创建并创建")
      console.log(this.deviceDetail[id])
    }
    console.log(this.deviceDetail[id])
    this.setData({
      devicePos: parseInt(event.currentTarget.id),
      swMap: this.deviceDetail[id].swMap,
      intensity: this.deviceDetail[id].intensity,
      tem: this.deviceDetail[id].tem,
      humidity: this.deviceDetail[id].humidity,
      sensorStatus: this.deviceDetail[id].sensorStatus
    })

  },
  gatherCmd: function (pos, value) {
    var msg = new Object()
    msg = this.data.swMap;
    msg["Fan"] = this.data.swMap["Fan"]
    msg["Led1"] = this.data.swMap["Led1"]
    msg["Led2"] = this.data.swMap["Led2"]
    msg["Led3"] = this.data.swMap["Led3"]
    msg["destDevice"] = this.data.deviceList[this.data.devicePos].deviceId
    switch (pos) {
      case 0:
        msg["Fan"] = value
        break;
      case 1:
        msg["Led1"] = value
        break;
      case 2:
        msg["Led2"] = value
        break;
      case 3:
        msg["Led3"] = value
        break;
    }

    return JSON.stringify(msg)
  },


  setViewFromJson: function (data) {

    this.deviceId = data["BaseBoard"]
    this.deviceDetail[data.BaseBoard] = new Object()
    this.deviceDetail[data.BaseBoard].swMap = { "Fan": data["Fan"], "Led1": data["Led1"], "Led2": data["Led2"], "Led3": data["Led3"] }
    this.deviceDetail[data.BaseBoard].intensity = data["Intensity"]

    this.deviceDetail[data.BaseBoard].tem = parseFloat(data["Temperature"]).toFixed(2)
    this.deviceDetail[data.BaseBoard].humidity = parseFloat(data["Humidity"]).toFixed(2)
    this.deviceDetail[data.BaseBoard].sensorStatus = { "Fan": data["FanOk"], "Is": data["IsOk"], "Tah": data["TahOk"] }
    if(data.BaseBoard === this.data.deviceList[this.data.devicePos].deviceId){
      this.setData({
        swMap: { "Fan": data["Fan"], "Led1": data["Led1"], "Led2": data["Led2"], "Led3": data["Led3"] },
        BaseBoard: data["BaseBoard"],
        intensity: data["Intensity"],
        tem: parseFloat(data["Temperature"]).toFixed(2),
        humidity: parseFloat(data["Humidity"]).toFixed(2),
        sensorStatus: { "Fan": data["FanOk"], "Is": data["IsOk"], "Tah": data["TahOk"] }

      })
    }
    console.log(this.deviceDetail)

  },
  socketConnect: function () {
    var code = this.wxcode
    var that = this
    if (this.isSocketOpen) {
      wx.closeSocket()
    }
    wx.connectSocket({
      url: 'wss://ks.risid.com/wss/WebSocketHandler.ashx?code=' + code,
      fail: function () {
        that.toastAlarm("连接失败！")
      }
    })
  },
  onPullDownRefresh: function () {
    this.getUserId()
    this.getDeviceList()
    wx.stopPullDownRefresh()
  },
  toastAlarm: function (msg) {
    wx.showToast({
      title: msg,
      image: "../../image/alarm.png"
    })
  },

  swChange: function (e) {
    var id = e.target.id
    var value = e.detail.value
    if (!this.isSocketOpen) {

      return;
    }
    var msg = this.gatherCmd(this.swPosMap[e.target.id], value)
    console.log(this.swPosMap[e.target.id])
    console.log(value)

    if (value != this.data.swMap[id]) {
      if (this.isSocketOpen) {
        console.log(msg)
        wx.sendSocketMessage({
          data: msg
        })
      }
    }
  },
  // 获取设备列表
  getDeviceList: function () {
    var that = this
    wx.request({
      url: "https://ks.risid.com/GetDevices.aspx",
      data: {
        code: that.wxcode
      },
      success: function (res) {
        console.log(res.data)
        var arr = res.data
        var deviceList = new Array()
        for (var i = 0; i < arr.length; i++) {
          deviceList[i] = new Object()
          deviceList[i]["id"] = i + 1
          deviceList[i]["deviceId"] = arr[i]
          wx.setStorageSync(arr[i], that.liveData)
        }
        console.log(deviceList)
        that.setData({
          deviceList: deviceList
        })
      }
    })
  },
  // 获取用户id并连接socket
  getUserId: function () {
    var that = this
    wx.login({
      success: function (res) {
        if (res.code) {
          wx.request({
            url: 'https://ks.risid.com/GetUID.aspx',
            data: {
              code: res.code
            },
            success: function (s) {
              console.log(s.data)
              wx.setStorageSync("code", s.data)
              that.wxcode = s.data
              that.socketConnect()
            }
          })

        } else {
          this.toastAlarm("获取用户ID失败！")
        }
      }
    });
  },
  // 发送msg
  socketSendMsg: function (msg) {
    try {
      if (this.isSocketOpen) {
        wx.sendSocketMessage({
          data: JSON.stringify(msg),
        })
      } else {
        this.toastAlarm("连接错误！")
      }

    } catch (e) {
      this.toastAlarm("连接错误！")
    }
  },
  onLoad: function () {
    var that = this;

    
    try {
      var value = wx.getStorageSync('code')
      console.log(value + "  from storge")
      if (value.length === 32) {
        this.wxcode = value
        this.socketConnect()
      } else {
        this.getUserId()
      }
    } catch (e) {
      this.getUserId()
    }
    that.getDeviceList()
    wx.onSocketError(function (res) {
      that.toastAlarm("连接失败！")
    })
    wx.onSocketOpen(function (res) {
      console.log('WebSocket连接已打开！')
      that.isSocketOpen = true
    })
    wx.onSocketMessage(function (res) {
      console.log(res.data)
      var data = JSON.parse(res.data)
      if (data["action"] != undefined) {
        if (data["action"] === "abort") {
          that.toastAlarm("设备不在线")

        }
        if (data["action"] === "allowBind") {
          wx.showToast({
            title: '绑定成功！',
          })

        }
        if (data["action"] === "forbid") {
          that.toastAlarm("设备拒绝连接")
          return
        }
        if (data["action"] === "exist") {
          that.toastAlarm("设备已经绑定")

        }
        if (data["action"] === "unbound") {
          wx.showToast({
            title: '解绑成功',
          })
        }
        that.getDeviceList()
        return
      }
      if (data["status"] != undefined) {
        if (data["status"] === "online") {
          that.deviceStatus[data.deviceId] = true
          that.setData({
            deviceStatus: that.deviceStatus
          })
        }
        if (data["status"] === "close") {
          that.deviceStatus[data.deviceId] = false
          that.setData({
            deviceStatus: that.deviceStatus
          })
        }

      }

      if (data["BaseBoard"] != undefined) {
        that.setViewFromJson(data)
        that.deviceStatus[data["BaseBoard"]] = true
        that.setData({
          deviceStatus: that.deviceStatus
        })

        if (that.liveData["intensity"][0] === 0) {
          that.liveData["intensity"].splice(0, 1)
          that.liveData["time"].splice(0, 1)
          that.liveData["humidity"].splice(0, 1)
          that.liveData["temperature"].splice(0, 1)
        }
        if (that.liveData["intensity"].length > 9) {
          // 移除最旧的
          that.liveData["intensity"].splice(0, 1)
          that.liveData["time"].splice(0, 1)
          that.liveData["humidity"].splice(0, 1)
          that.liveData["temperature"].splice(0, 1)
        }
        that.liveData["intensity"].push(data.Intensity)
        that.liveData["time"].push(data.Date.substring(10))
        that.liveData["humidity"].push(data.Humidity)
        that.liveData["temperature"].push(data.Humidity)
        wx.setStorageSync(data["BaseBoard"], that.liveData)
        var arr = getCurrentPages()
        // 图表显示页开启时，跨页调用数据
        if (arr.length === 2 && arr[1].deviceId === data["BaseBoard"]) {
          arr[1].updateData()
        }

      }
    })
    wx.onSocketClose(function (res) {
      console.log('WebSocket 已关闭！')
      that.isSocketOpen = false
    })
  },
  formSubmit: function (e) {
    wx.closeSocket();
  },
  addDevice: function (e) {
    var that = this
    wx.scanCode({
      onlyFromCamera: true,
      success: (res) => {
        var deviceId = res.result
        if (deviceId.length != 32) {
          console.log(deviceId)
          that.toastAlarm("错误的设备码")
        } else {
          console.log(deviceId)
          var msg = new Object()
          msg["action"] = "bind"
          msg["deviceId"] = deviceId
          that.socketSendMsg(msg)
        }
      }
    })
  }
})
