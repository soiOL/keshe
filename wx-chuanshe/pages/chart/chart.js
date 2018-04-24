// pages/chart.js
var wxCharts = require('../../utils/wxcharts.js')
Page({

  /**
   * 页面的初始数据
   */
  data: {
    chartList: [
      { "id": 0, "content": "intensity", "name": "光强", "unit": "lux", "color": "#ffbe00" },
      { "id": 1, "content": "temperature", "name": "温度", "unit": "℃", "color": "#f76260" },
      { "id": 2, "content": "humidity", "name": "湿度", "unit": "%", "color": "#10aeff" }]
  },
  chartMap: { "intensity": 0, "temperature": 1, "humidity": 2 },
  touchHandler: function (e) {

    var lineChart = this.lineCharts[this.chartMap[e.target.target]]
    lineChart.scrollStart(e);
  },
  moveHandler: function (e) {
    var lineChart = this.lineCharts[this.chartMap[e.target.target]]
    lineChart.scroll(e);
  },
  touchEndHandler: function (e) {
    var id = this.chartMap[e.target.target]
    var lineChart = this.lineCharts[this.chartMap[e.target.target]]
    var that = this
    lineChart.scrollEnd(e);
    lineChart.showToolTip(e, {
      format: function (item, category) {
        return category + ' ' + item.name + ':' + item.data
      }
    });

  },
  windowsWidth: 320,
  wantTime: "live",
  wxcode: "",
  timeChange: function (e) {
    console.log(e)
    var time = e.detail.value
    this.wantTime = time
    if (this.wantTime === "live") {
      this.updateData()
    }
    if (this.wantTime === "ahour") {
      this.getInfoData(1)
    }
    if (this.wantTime === "aday") {
      this.getInfoData(24)
    }

  },
  getInfoData: function (time) {
    var that = this
    wx.request({
      url: "https://ks.risid.com/GetInfoByHour.aspx",
      data: {
        code: that.wxcode,
        deviceId: that.deviceId,
        hour: time
      },
      success: function (res) {
        var data = res.data
        console.log(data)
        if (data.error == 0) {
          if (data.info.length > 0) {
            that.updateChartByNet(data.info)
          } else {
            that.toastAlarm("该时段没有数据")
          }

        } else {
          that.toastAlarm("获取数据失败")
        }
      },
      complete: function () {
        wx.stopPullDownRefresh()
      }
    })
  },
  onPullDownRefresh: function () {
    console.log("刷新")
    if (this.wantTime === "live") {
      this.updateData()
    }
    if (this.wantTime === "ahour") {
      this.getInfoData(1)
    }
    if (this.wantTime === "aday") {
      this.getInfoData(24)
    }
  },
  toastAlarm: function (msg) {
    wx.showToast({
      title: msg,
      image: "../../image/alarm.png"
    })
  },
  updateChartByNet: function (data) {
    var charList = this.data.chartList;
    var infoData = { "intensity": [], "temperature": [], "humidity": [], "time": [] }
    for (var i = 0; i < data.length; i++) {
      infoData.intensity.push(data[i].Intensity)
      infoData.temperature.push(data[i].Temperature)
      infoData.humidity.push(data[i].Humidity)
      infoData.time.push(data[i].Date.substring(10))
    }
    for (var i = 0; i < 3; i++) {
      console.log("嘻嘻")
      console.log(charList[i])
      var series = [{
        name: charList[i].name,
        data: infoData[charList[i].content],
        color: charList[i].color,
        format: function (val) {
          
          return val;
        }
      }]
      console.log(series)
      this.lineCharts[i].updateData({
        categories: infoData.time,
        series: series
      });
      console.log(this.lineCharts[i])
    }
  },
  deviceId: "",
  lineCharts: new Array(),
  liveData: { "intensity": [], "temperature": [], "humidity": [], "time": [] },
  updateData: function (data) {
    var charList = this.data.chartList;
    console.log(this.wantTime)
    if (this.wantTime === "live") {
      var data = wx.getStorageSync(this.deviceId)
      for (var i = 0; i < this.lineCharts.length; i++) {
        console.log("开始")
        var series = [{
          name: charList[i].name,
          data: data[charList[i].content],
          color: charList[i].color,
          format: function (val, name) {
            return val + name;
          }
        }]
        console.log(series)
        this.lineCharts[i].updateData({
          categories: data["time"],
          series: series
        });
        console.log(this.lineCharts[i])
      }
    }
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.wxcode = wx.getStorageSync("code")
    var pageName = "图表"
    if (options.id != undefined) {
      pageName = "设备" + options.id + "图表"
    }
    wx.setNavigationBarTitle({
      title: pageName,
    })
    this.deviceId = options.deviceId
    this.windowsWidth = wx.getSystemInfoSync().windowWidth
    this.initCharts()


  },

  initCharts: function () {
    var width = this.windowsWidth
    // var simulationData = this.createSimulationData()
    var that = this
    var list = this.data.chartList
    var data = wx.getStorageSync(this.deviceId)
    for (var i = 0; i < list.length; i++) {
      console.log(that.liveData[that.data.chartList[i].content])
      console.log(that.liveData["time"])
      var chart = new wxCharts({
        canvasId: list[i].content,
        type: 'line',
        categories: data["time"],
        animation: false,
        enableScroll: true,
        series: [{
          name: list[i].name,
          data: data[this.data.chartList[i].content],
          color: list[i].color,
          format: function (val, name) {
            return val + name;
          }
        }],
        xAxis: {
          disableGrid: true
        },
        yAxis: {
          title: list[i].unit,
          format: function (val) {
            return val;
          },
          min: 0
        },
        width: width,
        height: 200,
        dataLabel: false,
        dataPointShape: true,
        extra: {
          lineStyle: 'curve'
        }
      });
      this.lineCharts.push(chart)
    }

    console.log(this.lineCharts)
  },
  cc: function(e){
    console.log(e.target.target)
  },
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },



})