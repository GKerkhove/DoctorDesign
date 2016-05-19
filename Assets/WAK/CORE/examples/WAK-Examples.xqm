xquery version "3.0";
module namespace WAK-Cars="http://WAK.com/api/cars";
import module namespace functx = "http://www.functx.com";
import module namespace xqjson="http://xqilla.sourceforge.net/lib/xqjson";
declare namespace output="http://www.w3.org/2010/xslt-xquery-serialization";

(: WAK Examples :)

(: Base URI : http://wak-api.unity3dassets.com:8080/exist/restxq :)
 
(: Hardcoded Path Example :)
 
declare
    %rest:GET
    %rest:path("/WAK/v1/cars/honda")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars1() 
{   
    <resource>
        <model>civic</model>
        <model>accord</model>
        <model>pilot</model>
    </resource>
};


(: URI Template Example :)

declare
    %rest:GET
    %rest:path("/WAK/v2/cars/{$make}")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars2($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else
        <resource/>
};


(: Query Parameters :)

declare
    %rest:GET
    %rest:path("/WAK/v2/cars")
    %rest:query-param("make","{$make}")
    %rest:query-param("sort","{$sort}","ascending")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars2-qp($make as xs:string*, $sort as xs:string*) 
{   
    if($make eq 'honda') then
    (
        let $models := (<model>civic</model>,<model>accord</model>,<model>pilot</model>)
        
        return
            <response>
            {
                if($sort eq 'descending') then
                    for $model in $models
                    order by $model/text() descending
                    return $model
                else
                    for $model in $models
                    order by $model/text() ascending
                    return $model
            }
            </response>
    )    
    else
        <response/>
};


(: Not Found 404 :)

declare
    %rest:GET
    %rest:path("/WAK/v3/cars/{$make}")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars3($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else
        <rest:response>
            <http:response status="404"/>
        </rest:response>
};


(: Not Found 404 with resource :)

declare
    %rest:GET
    %rest:path("/WAK/v4/cars/{$make}")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars4($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else if(contains($make,'.')) then
         (<rest:response>
            <http:response status="400"/>
        </rest:response>,
        <resource>
            <message>Invalid request.</message>
        </resource>)
    else
        (<rest:response>
            <http:response status="404"/>
        </rest:response>,
        <resource>
            <message>Sorry, not found.</message>
        </resource>)
};


(: Not Found 200 :)

declare
    %rest:GET
    %rest:path("/WAK/v5/cars/{$make}")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars5($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else
        (<rest:response>
            <http:response status="200"/>
        </rest:response>,
        <resource>
            <fault>
                <message>Sorry, not found.</message>
            </fault>
        </resource>)
};


(:  Content Negotiation Example  :)

declare
    %rest:GET
    %rest:path("/WAK/v6/cars/{$make}")
    %rest:produces("application/json")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:get-cars6-json($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else
        (<rest:response>
            <http:response status="200"/>
        </rest:response>,
        <resource>
            <fault>
                <message>Sorry, not found.</message>
            </fault>
        </resource>)
};

declare
    %rest:GET
    %rest:path("/WAK/v6/cars/{$make}")
    %rest:produces("application/xml")
    %output:media-type("application/xml")
    %output:method("xml")
function WAK-Cars:get-cars6-xml($make as xs:string*) 
{   
    if($make eq 'honda') then
        <resource>
            <model>civic</model>
            <model>accord</model>
            <model>pilot</model>
        </resource>
    else
        (<rest:response>
            <http:response status="200"/>
        </rest:response>,
        <resource>
            <fault>
                <message>Sorry, not found.</message>
            </fault>
        </resource>)
};

declare
    %rest:GET
    %rest:path("/WAK/v6/cars/{$make}")
    %rest:produces("image/png")
    %output:media-type("image/png")
    %output:method("binary")
function WAK-Cars:get-cars6-png($make as xs:string*) 
{   
    if($make eq 'honda') then
        util:binary-doc('/db/apps/WAK/resources/honda1.jpg')
    else
        <rest:response>
            <http:response status="404"/>
        </rest:response>
};

declare
    %rest:GET
    %rest:path("/WAK/v7/cars/{$make}")
    %rest:header-param("X-Auth", '{$auth}')
    %rest:produces("image/png")
    %output:media-type("image/png")
    %output:method("binary")
function WAK-Cars:get-cars7-png-auth($make as xs:string*, $auth as xs:string*) 
{   
    if(not($auth) or $auth ne 'secret-password') then
        <rest:response>
            <http:response status="401"/>
        </rest:response>
    else if($make eq 'honda') then
        util:binary-doc('/db/apps/WAK/resources/honda1.jpg')
    else
        <rest:response>
            <http:response status="404"/>
        </rest:response>
};



declare
    %rest:POST('{$payload}')
    %rest:path("/WAK/v1/cars")
    %output:media-type("application/json")
    %output:method("json")
function WAK-Cars:http-method-post($payload as xs:string) 
{   
    <response>
        <added>{$payload}</added>
    </response>
};
