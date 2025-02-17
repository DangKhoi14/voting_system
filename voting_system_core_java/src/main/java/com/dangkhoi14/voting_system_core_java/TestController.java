package com.dangkhoi14.voting_system_core_java;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class TestController {
    
    @RequestMapping("/test")
    public String test() {
        return "Hello World!";
    }
}
