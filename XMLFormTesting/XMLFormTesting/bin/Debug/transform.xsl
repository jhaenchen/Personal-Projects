<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h2><xsl:value-of select="form/@name"/></h2>
  <xsl:for-each select="form/section">
	  <table border="1">
		<tr bgcolor="#9acd32">
		  <th>Section</th>
		  <th>Field</th>
		  <th>Value</th>
		</tr>
		<xsl:for-each select="./field">
			<tr>
				<td><xsl:value-of select="../@name"/></td>
				<td><xsl:value-of select="@name"/></td>
				<td><xsl:value-of select="."/></td>
			</tr>
		</xsl:for-each>
	  </table>
  </xsl:for-each>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>