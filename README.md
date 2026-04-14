# AOTVI AOI 标注系统

## 📌 项目简介

本系统用于 AOI（自动光学检测）缺陷标注与结果判定，主要应用于产线环境。

主要功能：

* 扫码枪输入 Lot
* Lot 合法性校验
* AOI 图片缺陷框绘制
* 点击缺陷切换 NG / OK
* 数据库存储
* MES 接口上传
* 实时日志显示
* DB / MES 网络状态检测

---

## 🧱 项目架构

AOTVI/
├─ AOTVI.UI        // WinForms 界面层
├─ AOTVI.BLL       // 业务逻辑层
├─ AOTVI.DAL       // 数据访问层
├─ AOTVI.Models    // 实体模型
├─ AOTVI.Common    // 工具类（日志/配置/网络等）

---

## 📐 分层设计规范

* UI 层

  * 仅负责界面展示与交互
  * 不编写业务逻辑

* BLL 层

  * 处理业务逻辑
  * 统一异常处理
  * 统一记录日志

* DAL 层

  * 仅负责数据库访问
  * 不记录日志，仅抛异常

* Common 层

  * 提供公共工具类（Log / Config / Net 等）

---

## ⚙️ 运行环境

* .NET Framework 4.7.2
* SQL Server
* Windows（产线环境）

---

## 🔧 配置说明（App.config）

### MES 接口配置

<add key="MesUrl" value="http://xxx/api/aoi" />

### 数据库连接

<connectionStrings>
  <add name="DbConn"
       connectionString="server=127.0.0.1;database=AAA;uid=xxx;password=xxx;" />
</connectionStrings>

⚠️ 注意：

* 禁止提交真实生产数据库账号密码到 GitHub
* 建议使用本地配置或测试环境配置

---

## 🚀 启动方式

1. 打开 AOTVI.sln
2. 设置 AOTVI.UI 为启动项目
3. 修改 App.config 中数据库连接
4. 运行程序

---

## 🧪 系统流程

1. 扫码 Lot
2. 系统进行 Lot 校验
3. 加载缺陷数据
4. 显示 AOI 图像与缺陷框
5. 点击缺陷切换 NG / OK
6. 点击确认提交：

   * 保存数据库
   * 上传 MES

---

## 📡 网络状态检测

系统每秒检测：

* 数据库端口
* MES 接口端口

UI 显示：

* 绿色：正常
* 红色：异常

---

## 📝 日志系统

* 使用 log4net
* 日志目录：/logs/yyyyMMdd.log
* UI 实时显示日志（最多 200 条）

---

## ⚠️ 异常处理策略

* DAL 层：只抛异常
* BLL 层：记录日志 + 抛业务异常
* UI 层：捕获异常并提示

全局异常捕获：

* UI 线程异常（Application.ThreadException）
* 非 UI 线程异常（AppDomain.UnhandledException）

---

## 🔄 Git 使用规范（简化）

提交代码：
git add .
git commit -m "fix: 修改说明"
git push

分支建议：

* main：产线稳定版本
* dev：开发版本

---

## 🧯 产线注意事项（重要）

* MES 接口必须设置超时（防止卡死）
* MES 失败不能丢数据（需重试机制）
* 数据库操作需设置 CommandTimeout
* 所有外部依赖必须异常保护
* UI 线程不得阻塞
* 日志必须完整可追溯

---

## 📌 后续优化方向

* MES 上传失败重试机制
* 离线缓存（断网补传）
* 数据库批量更新优化
* 操作员权限管理
* AOI 图片加载优化

---

## 👨‍🔧 维护说明

系统异常排查步骤：

1. 查看 logs 日志文件
2. 检查数据库连接状态
3. 检查 MES 接口状态
4. 确认网络是否正常
5. 联系工程师处理

---
