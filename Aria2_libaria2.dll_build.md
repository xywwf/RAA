
<p>
	<br />
</p>
<pre class="prettyprint lang-cs">Windows中Aria2动态编译和使用
介绍
Aria2 是一个轻量的多协议多源命令行下载工具，支持 HTTP/HTTPS, FTP, SFTP, BItTorrent, Metalink。并提供 JSON-RPC 和 XML-RPC 接口调用。
总而言之，Aria2 是一个开源的下载库，可以集成到程序中获得下载功能
编译
Aria2 需要用到 mingw-w64 的交叉编译工具链来编译出来 Windows 下的二进制，所以要首先安装一个 Linux 系统，推荐用 Ubuntu
新建一个 setup.sh 脚本，来执行环境的安装和相关依赖库的编译  </pre>
<pre class="prettyprint lang-cs">#! /bin/bash

# 改成 x86_64-w64-mingw32 来编译64位版本
export HOST=i686-w64-mingw32

# It would be better to use nearest ubuntu archive mirror for faster
# downloads.
# RUN sed -ie 's/archive\.ubuntu/jp.archive.ubuntu/g' /etc/apt/sources.list
# 安装编译环境
apt-get update &amp;&amp; \
apt-get install -y make binutils autoconf automake autotools-dev libtool pkg-config git curl dpkg-dev gcc-mingw-w64 autopoint libcppunit-dev libxml2-dev libgcrypt11-dev lzip

# 下载依赖库
if [ ! -f "gmp-6.1.2.tar.lz" ]; then 
	curl -L -O https://gmplib.org/download/gmp/gmp-6.1.2.tar.lz 
fi

if [ ! -f "expat-2.2.0.tar.bz2" ]; then 
	curl -L -O http://downloads.sourceforge.net/project/expat/expat/2.2.0/expat-2.2.0.tar.bz2
fi

if [ ! -f "sqlite-autoconf-3160200.tar.gz" ]; then 
	curl -L -O https://www.sqlite.org/2017/sqlite-autoconf-3160200.tar.gz
fi

if [ ! -f "zlib-1.2.11.tar.gz" ]; then 
	curl -L -O http://zlib.net/zlib-1.2.11.tar.gz
fi

if [ ! -f "c-ares-1.12.0.tar.gz" ]; then 
	curl -L -O https://c-ares.haxx.se/download/c-ares-1.12.0.tar.gz
fi

if [ ! -f "libssh2-1.8.0.tar.gz" ]; then 
	curl -L -O http://libssh2.org/download/libssh2-1.8.0.tar.gz
fi

# 动态编译 gmp
tar xf gmp-6.1.2.tar.lz &amp;&amp; \
cd gmp-6.1.2 &amp;&amp; \
./configure --enable-shared --disable-static --prefix=/usr/local/$HOST --host=$HOST --disable-cxx --enable-fat CFLAGS="-mtune=generic -O2 -g0" &amp;&amp; \
make install

# 动态编译 expat
cd ..
tar xf expat-2.2.0.tar.bz2 &amp;&amp; \
cd expat-2.2.0 &amp;&amp; \
./configure --enable-shared --disable-static --prefix=/usr/local/$HOST --host=$HOST --build=`dpkg-architecture -qDEB_BUILD_GNU_TYPE` &amp;&amp; \
make install

# 动态编译 sqlite3
cd ..
tar xf sqlite-autoconf-3160200.tar.gz &amp;&amp; cd sqlite-autoconf-3160200 &amp;&amp; \
./configure --enable-shared --disable-static --prefix=/usr/local/$HOST --host=$HOST --build=`dpkg-architecture -qDEB_BUILD_GNU_TYPE` &amp;&amp; \
make install

# 动态编译 zlib
cd ..
tar xf zlib-1.2.11.tar.gz &amp;&amp; \
cd zlib-1.2.11
export BINARY_PATH=/usr/local/$HOST/bin
export INCLUDE_PATH=/usr/local/$HOST/include
export LIBRARY_PATH=/usr/local/$HOST/lib
make install -f win32/Makefile.gcc PREFIX=$HOST- SHARED_MODE=1

# 动态编译 c-ares
cd ..
tar xf c-ares-1.12.0.tar.gz &amp;&amp; \
cd c-ares-1.12.0 &amp;&amp; \
./configure --enable-shared --disable-static --without-random --prefix=/usr/local/$HOST --host=$HOST --build=`dpkg-architecture -qDEB_BUILD_GNU_TYPE` LIBS="-lws2_32" &amp;&amp; \
make install

# 动态编译 libssh2
cd ..
tar xf libssh2-1.8.0.tar.gz &amp;&amp; \
cd libssh2-1.8.0 &amp;&amp; \
./configure --enable-shared --disable-static --prefix=/usr/local/$HOST --host=$HOST --build=`dpkg-architecture -qDEB_BUILD_GNU_TYPE` --without-openssl --with-wincng LIBS="-lws2_32" &amp;&amp; \
make install</pre>
<p>
	下载 Aria2 代码并解压，修改 aria2\mingw-config 文件，增加 --enable-libaria2 编译选项来生成 libaria2 库，并修改ARIA2_STATIC=no 来关闭静态编译，如果想在 windows xp 上运行，则还要将 --with-libssh2 改成 --without-libssh2，否则会出现 bcrypt.dll 缺失报错。
</p>
<p>
	<br />
</p>
<p>
	新建一个 build.sh 来编译 Aria2 库
</p>
<p>
	<br />
</p>
<p>
	<br />
</p>
<pre class="prettyprint lang-cs">#! /bin/bash

# 改成 x86_64-w64-mingw32 来编译64位版本
export HOST=i686-w64-mingw32
cd aria2 &amp;&amp; autoreconf -i &amp;&amp; ./mingw-config &amp;&amp; make install &amp;&amp; $HOST-strip /usr/local/$HOST/bin/libaria2-0.dll
</pre>
<pre class="prettyprint lang-cs">
</pre>
<pre class="prettyprint lang-cs">
<ul style="font-family:&quot;font-size:18px;vertical-align:baseline;color:#555555;background-color:#FFFFFF;">
	
	<li style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">
		编译完成后，将两个&nbsp;gcc&nbsp;的运行库复制到&nbsp;/usr/local/i686-w64-mingw32/bin&nbsp;中，路径为&nbsp;/usr/lib/gcc/i686-w64-mingw32/5.3-win32/libgcc_s_sjlj-1.dll&nbsp;和&nbsp;/usr/lib/gcc/i686-w64-mingw32/5.3-win32/libstdc++-6.dll
	</li>

	<li style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">
		最终，把&nbsp;/usr/local/i686-w64-mingw32&nbsp;文件夹复制到&nbsp;Windows&nbsp;中，这个文件夹中的文件就是&nbsp;Windows&nbsp;所需要的二进制
	</li>

</ul>

<h4 id="使用" style="font-family:&quot;font-size:1.2em;vertical-align:baseline;color:#555555;background-color:#FFFFFF;">
	<a href="http://wangjie.rocks/2017/02/12/build-aria2/#%E4%BD%BF%E7%94%A8" class="headerlink"></a>使用
</h4>

<ul style="font-family:&quot;font-size:18px;vertical-align:baseline;color:#555555;background-color:#FFFFFF;">
	
	<li style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">
		编译完成后，aria2c.exe&nbsp;可以运行，但是我们自己的程序还无法加载&nbsp;libaria2-0.dll，原因是这个库接口的导出方式的问题，要想在程序中能够使用&nbsp;libaria2-0.dll&nbsp;的接口，还需要对&nbsp;aria2\src\includes\aria2\aria2.h&nbsp;和&nbsp;arai2\src\aria2api.cc&nbsp;中的接口进行修改，将所有接口都加上&nbsp;extern "C"&nbsp;来导出，并且修改接口中的&nbsp;STL&nbsp;和 自定义class&nbsp;参数，去掉或改成指针的方式，例如
	</li>

</ul>
<span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="comment" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#808080;">// int libraryInit();</span></span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="comment" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#808080;">// 改成</span></span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">extern</span> <span class="string" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#6A8759;">"C"</span> <span class="function" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;"><span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">int</span> <span class="title" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#FFC66D;">libraryInit</span><span class="params" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">()</span></span>;</span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"></span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="comment" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#808080;">// Session* sessionNew(const KeyVals&amp; options, const SessionConfig&amp; config);</span></span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="comment" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#808080;">// 改成</span></span> <span class="line" style="font-family:&quot;font-size:14px;vertical-align:baseline;color:#A9B7C6;background-color:#282B2E;"><span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">extern</span> <span class="string" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#6A8759;">"C"</span> <span class="function" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">Session* <span class="title" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#FFC66D;">sessionNewEx</span><span class="params" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">(<span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">bool</span> keepRunning, <span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">bool</span> useSignalHandler, DownloadEventCallback downloadEventCallback, <span class="keyword" style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;color:#CC7832;">void</span>* userData)</span></span>;</span> 
<ul style="font-family:&quot;font-size:18px;vertical-align:baseline;color:#555555;background-color:#FFFFFF;">
	
	<li style="font-weight:inherit;font-style:inherit;font-family:inherit;vertical-align:baseline;">
		修改完成后重新编译，然后在程序中用&nbsp;LoadLibrary&nbsp;和&nbsp;GetProcAddresss&nbsp;来获取各个接口，就可以使用&nbsp;libaria2&nbsp;的下载功能了
	</li>

</ul>
</pre>
<pre class="prettyprint lang-cs">
</pre>
<pre class="prettyprint lang-cs">
</pre>
<pre class="prettyprint lang-cs">
</pre>
<p>
	<br />
</p>